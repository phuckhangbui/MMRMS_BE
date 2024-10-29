using AutoMapper;
using BusinessObject;
using Common.Enum;
using DAO;
using DTOs.Machine;
using DTOs.MachineSerialNumber;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;

namespace Repository.Implement
{
    public class MachineRepository : IMachineRepository
    {

        private readonly IMapper _mapper;

        public MachineRepository(IMapper mapper)
        {
            _mapper = mapper;
        }


        public async Task<IEnumerable<MachineDto>> GetMachineList()
        {
            var list = await MachineDao.Instance.GetMachineListWithCategory();

            var resultList = new List<MachineDto>();

            if (list.IsNullOrEmpty())
            {
                return null;
            }

            foreach (var product in list)
            {
                var productDto = _mapper.Map<MachineDto>(product);

                productDto.Quantity = product.MachineSerialNumbers?.Count;

                resultList.Add(productDto);
            }

            return resultList;
        }

        public async Task<MachineDto> GetMachine(int productId)
        {
            var product = await MachineDao.Instance.GetMachine(productId);

            if (product == null)
                return null;

            return _mapper.Map<MachineDto>(product);
        }

        public async Task<DisplayMachineDetailDto> GetMachineDetail(int productId)
        {
            var product = await MachineDao.Instance.GetMachineDetail(productId);

            if (product == null)
            {
                return null;
            }

            var productDetail = _mapper.Map<DisplayMachineDetailDto>(product);

            //Quantity availble
            var machineSerialNumbers = await MachineSerialNumberDao.Instance.GetMachineSerialNumbersByMachineIdAndStatus(productId, MachineSerialNumberStatusEnum.Available.ToString());
            productDetail.Quantity = machineSerialNumbers.Count();

            var prices = machineSerialNumbers
                .Select(s => s.ActualRentPrice ?? 0)
                .OrderBy(s => s)
                .ToList();
            productDetail.RentPrices = prices;

            return productDetail;
        }

        public async Task<IEnumerable<MachineSerialNumberDto>> GetMachineNumberList(int productId)
        {
            var product = await MachineDao.Instance.GetMachineWithSerialMachineNumber(productId);

            if (product == null)
            {
                return null;
            }

            return _mapper.Map<IEnumerable<MachineSerialNumberDto>>(product.MachineSerialNumbers);
        }

        public async Task<MachineDto> CreateMachine(CreateMachineDto createMachineDto)
        {
            var product = _mapper.Map<Machine>(createMachineDto);

            if (product == null)
            {
                return null;
            }

            var componentMachines = new List<MachineComponent>();

            if (!createMachineDto.ExistedComponentList.IsNullOrEmpty())
            {
                foreach (AddExistedComponentToMachine component in createMachineDto.ExistedComponentList)
                {
                    var componentMachine = _mapper.Map<MachineComponent>(component);
                    componentMachines.Add(componentMachine);
                }
            }

            product.MachineComponents = componentMachines;

            product.DateCreate = DateTime.Now;
            product.Status = MachineStatusEnum.Locked.ToString();

            List<Tuple<Component, int, bool>> componentsTuple = new List<Tuple<Component, int, bool>>();

            if (!createMachineDto.NewComponentList.IsNullOrEmpty())
            {
                foreach (var component in createMachineDto.NewComponentList)
                {
                    Component Component = new Component
                    {
                        ComponentName = component.ComponentName.Trim(),
                        AvailableQuantity = null,
                        Price = component.Price,
                        Status = ComponentStatusEnum.NoQuantity.ToString(),
                        DateCreate = DateTime.Now,
                    };

                    componentsTuple.Add(new Tuple<Component, int, bool>(Component, component.Quantity, component.IsRequiredMoney));

                }
            }
            var productImages = new List<MachineImage>();

            if (!createMachineDto.ImageUrls.IsNullOrEmpty())
            {
                bool isFirstImage = true;

                foreach (var imageUrl in createMachineDto.ImageUrls)
                {
                    var productImage = new MachineImage
                    {
                        MachineImageUrl = imageUrl.Url,
                        IsThumbnail = isFirstImage
                    };

                    productImages.Add(productImage);
                    isFirstImage = false;
                }
            }

            product.MachineImages = productImages;

            product = await MachineDao.Instance.CreateMachine(product, componentsTuple);


            return _mapper.Map<MachineDto>(product);
        }

        public async Task<bool> IsMachineExisted(int productId)
        {
            return await MachineDao.Instance.IsMachineExisted(productId);
        }

        public async Task<bool> IsMachineExisted(string name)
        {
            return await MachineDao.Instance.IsMachineExisted(name);
        }

        public async Task<bool> IsMachineModelExisted(string model)
        {
            return await MachineDao.Instance.IsMachineModelExisted(model);
        }

        public async Task UpdateMachine(MachineDto productDto)
        {
            var product = _mapper.Map<Machine>(productDto);

            await MachineDao.Instance.UpdateAsync(product);
        }

        public async Task DeleteMachine(int productId)
        {
            await MachineDao.Instance.DeleteMachine(productId);
        }


        public async Task UpdateMachineAttribute(int productId, IEnumerable<CreateMachineAttributeDto> productAttributeDtos)
        {
            var product = await MachineDao.Instance.GetMachineDetail(productId);

            var productAttributes = new List<MachineAttribute>();

            foreach (var attributeDto in productAttributeDtos)
            {
                var attribute = new MachineAttribute
                {
                    MachineId = product.MachineId,
                    AttributeName = attributeDto.AttributeName,
                    Unit = attributeDto.Unit,
                    Specifications = attributeDto.Specifications
                };

                productAttributes.Add(attribute);
            }

            await MachineDao.Instance.UpdateMachineAttribute(product, productAttributes);
        }

        public async Task UpdateMachineComponent(int productId, ComponentList componentList)
        {
            var product = await MachineDao.Instance.GetMachineDetail(productId);

            var componentMachines = new List<MachineComponent>();

            if (!componentList.ExistedComponentList.IsNullOrEmpty())
            {
                foreach (AddExistedComponentToMachine component in componentList.ExistedComponentList)
                {
                    var componentMachine = _mapper.Map<MachineComponent>(component);
                    componentMachines.Add(componentMachine);
                }
            }

            product.MachineComponents = componentMachines;

            List<Tuple<Component, int, bool>> components = new List<Tuple<Component, int, bool>>();
            if (!componentList.NewComponentList.IsNullOrEmpty())
                foreach (var component in componentList.NewComponentList)
                {
                    Component Component = new Component
                    {
                        ComponentName = component.ComponentName.Trim(),
                        AvailableQuantity = null,
                        Price = component.Price,
                        Status = ComponentStatusEnum.NoQuantity.ToString(),
                        DateCreate = DateTime.Now,
                    };

                    components.Add(new Tuple<Component, int, bool>(Component, component.Quantity, component.IsRequiredMoney));
                }

            await MachineDao.Instance.UpdateMachineComponent(product, components, componentMachines);
        }

        public async Task ChangeMachineThumbnail(int productId, string imageUrlStr)
        {
            var productImage = new MachineImage
            {
                MachineImageUrl = imageUrlStr,
                MachineId = productId,
                IsThumbnail = true
            };

            await MachineDao.Instance.ChangeMachineThumbnail(productImage);
        }

        public async Task UpdateMachineImage(int productId, List<ImageList> imageList)
        {
            var product = await MachineDao.Instance.GetMachine(productId);
            bool isFirstImage = true;

            var productImages = new List<MachineImage>();

            foreach (var imageUrl in imageList)
            {
                var productImage = new MachineImage
                {
                    MachineId = productId,
                    MachineImageUrl = imageUrl.Url,
                    IsThumbnail = isFirstImage
                };

                productImages.Add(productImage);
                isFirstImage = false;
            }

            await MachineDao.Instance.AddMachineImages(productId, productImages);
        }

        public async Task UpdateMachineTerm(int productId, IEnumerable<CreateMachineTermDto> productTermDtos)
        {
            var product = await MachineDao.Instance.GetMachineDetail(productId);

            var productTerms = new List<MachineTerm>();

            foreach (var termDto in productTermDtos)
            {
                var term = new MachineTerm
                {
                    MachineId = product.MachineId,
                    Title = termDto.Title,
                    Content = termDto.Content
                };

                productTerms.Add(term);
            }

            await MachineDao.Instance.UpdateMachineTerm(product, productTerms);
        }

        public async Task<IEnumerable<MachineDto>> GetTop8LatestMachineList()
        {
            var products = await MachineDao.Instance.GetTop8LatestMachines();
            if (!products.IsNullOrEmpty())
            {
                return _mapper.Map<IEnumerable<MachineDto>>(products);
            }

            return [];
        }

        public async Task<IEnumerable<MachineReviewDto>> GetMachineReviews(List<int> productIds)
        {
            var productReviewList = new List<MachineReviewDto>();

            foreach (var productId in productIds)
            {
                var product = await MachineDao.Instance.GetMachineWithSerialMachineNumberAndMachineImages(productId);

                if (product != null)
                {
                    var productReview = _mapper.Map<MachineReviewDto>(product);

                    var thumbnailUrl = product.MachineImages
                        .FirstOrDefault(p => p.IsThumbnail == true)?.MachineImageUrl ?? string.Empty;
                    productReview.ThumbnailUrl = thumbnailUrl;

                    var machineSerialNumbers = await MachineSerialNumberDao.Instance.GetMachineSerialNumbersByMachineIdAndStatus(productReview.MachineId, MachineSerialNumberStatusEnum.Available.ToString());
                    if (!machineSerialNumbers.IsNullOrEmpty())
                    {
                        productReview.Quantity = machineSerialNumbers.Count();

                        var prices = machineSerialNumbers
                            .Select(s => s.ActualRentPrice ?? 0)
                            .OrderBy(s => s)
                            .ToList();
                        productReview.RentPrices = prices;

                        productReviewList.Add(productReview);
                    }
                }
            }

            return productReviewList;
        }
    }
}
