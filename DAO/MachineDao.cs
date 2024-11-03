using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Component = BusinessObject.Component;
using Machine = BusinessObject.Machine;
using MachineAttribute = BusinessObject.MachineAttribute;

namespace DAO
{
    public class MachineDao : BaseDao<Machine>
    {
        private static MachineDao instance = null;
        private static readonly object instacelock = new object();

        private MachineDao()
        {

        }

        public static MachineDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MachineDao();
                }
                return instance;
            }
        }

        public async Task<bool> IsMachineExisted(int productId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Machines.AnyAsync(p => p.MachineId == productId);

            }
        }

        public async Task<Machine> GetMachine(int productId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Machines
                    .Include(p => p.Category)
                    .Include(p => p.MachineImages)
                    .Include(p => p.MachineTerms)
                    .FirstOrDefaultAsync(p => p.MachineId == productId);

            }
        }


        public async Task<bool> IsMachineExisted(string name)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Machines.AnyAsync(p => p.MachineName.ToLower().Equals(name.Trim().ToLower()));

            }
        }

        public async Task<bool> IsMachineModelExisted(string model)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Machines.AnyAsync(p => p.Model.ToLower().Equals(model.Trim().ToLower()));

            }
        }

        public async Task<IEnumerable<Machine>> GetMachineListWithCategory()
        {
            using (var context = new MmrmsContext())
            {
                var list = await context.Machines
                    .Include(p => p.Category)
                    .Include(p => p.MachineImages)
                    .Include(p => p.MachineSerialNumbers)
                    .OrderByDescending(p => p.DateCreate)
                    .ToListAsync();

                return list;
            }
        }

        public async Task<Machine> GetMachineDetail(int productId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Machines.Include(p => p.Category)
                                             .Include(p => p.MachineImages)
                                             .Include(p => p.MachineAttributes)
                                             .Include(p => p.MachineTerms)
                                             .Include(p => p.MachineComponents)
                                             .ThenInclude(c => c.Component)
                                             .FirstOrDefaultAsync(p => p.MachineId == productId);
            }
        }

        public async Task<Machine> GetMachineWithSerialMachineNumber(int productId)
        {
            using (var context = new MmrmsContext())
            {
                var product = await context.Machines.Include(p => p.MachineSerialNumbers).FirstOrDefaultAsync(p => p.MachineId == productId);

                return product;

            }

        }

        public async Task DeleteMachine(int productId)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var product = await context.Machines.Include(p => p.Category)
                                            .Include(p => p.MachineImages)
                                            .Include(p => p.MachineAttributes)
                                            .Include(p => p.MachineComponents)
                                            .ThenInclude(c => c.Component)
                                            .FirstOrDefaultAsync(p => p.MachineId == productId);

                        foreach (var image in product.MachineImages)
                        {
                            DbSet<MachineImage> _dbSet = context.Set<MachineImage>();
                            _dbSet.Remove(image);

                        }

                        foreach (var attribute in product.MachineAttributes)
                        {
                            DbSet<MachineAttribute> _dbSet = context.Set<MachineAttribute>();
                            _dbSet.Remove(attribute);

                        }

                        foreach (var componentMachine in product.MachineComponents)
                        {
                            DbSet<MachineComponent> _dbSet = context.Set<MachineComponent>();
                            _dbSet.Remove(componentMachine);

                        }

                        product.MachineComponents = null;
                        product.MachineImages = null;
                        product.MachineAttributes = null;

                        DbSet<Machine> dbSet = context.Set<Machine>();
                        dbSet.Remove(product);

                        await context.SaveChangesAsync();

                        await transaction.CommitAsync();

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }

            }
        }

        public async Task<Machine> CreateMachine(Machine product, List<Tuple<Component, int, bool>>? newComponentList)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (!newComponentList.IsNullOrEmpty())
                        {
                            foreach (var tuple in newComponentList)
                            {
                                var component = tuple.Item1;
                                var quantity = tuple.Item2;
                                var isRequireMoney = tuple.Item3;

                                context.Components.Add(component);
                                await context.SaveChangesAsync();

                                var componentMachine = new MachineComponent()
                                {
                                    ComponentId = component.ComponentId,
                                    MachineId = product.MachineId,
                                    Quantity = quantity,
                                };

                                // Add the MachineComponent to the product's MachineComponents
                                product.MachineComponents.Add(componentMachine);
                            }
                        }

                        DbSet<Machine> _dbSet = context.Set<Machine>();
                        _dbSet.Add(product);
                        await context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        return product;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        public async Task UpdateMachineComponent(Machine product, List<Tuple<Component, int, bool>>? newComponentList, IEnumerable<MachineComponent>? componentMachines)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var oldMachineComponents = context.MachineComponents
                         .Where(cp => cp.MachineId == product.MachineId);

                        context.MachineComponents.RemoveRange(oldMachineComponents);
                        await context.SaveChangesAsync();

                        //Handle the new component list
                        if (!newComponentList.IsNullOrEmpty())
                        {
                            foreach (var tuple in newComponentList)
                            {
                                var component = tuple.Item1;
                                var quantity = tuple.Item2;
                                var isRequiredMoney = tuple.Item3;

                                context.Components.Add(component);
                                await context.SaveChangesAsync();

                                var componentMachine = new MachineComponent()
                                {
                                    ComponentId = component.ComponentId,
                                    MachineId = product.MachineId,
                                    Quantity = quantity,
                                };

                                // Add the MachineComponent to the product's MachineComponents
                                product.MachineComponents.Add(componentMachine);
                            }
                        }

                        // Append the additional MachineComponents to the product (if any)
                        var componentMachineList = new List<MachineComponent>();
                        if (!componentMachines.IsNullOrEmpty())
                        {
                            foreach (var cp in componentMachines)
                            {
                                cp.MachineId = product.MachineId;
                                componentMachineList.Add(cp);
                            }
                        }

                        context.MachineComponents.AddRange(componentMachineList);

                        context.Machines.Update(product);
                        await context.SaveChangesAsync();

                        await transaction.CommitAsync();

                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception(e.Message);
                    }
                }
            }
        }



        public async Task UpdateMachineAttribute(Machine product, IEnumerable<MachineAttribute> productAttributeLists)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var oldAttributeMachines = context.MachineAttributes
                       .Where(cp => cp.MachineId == product.MachineId);

                        context.MachineAttributes.RemoveRange(oldAttributeMachines);
                        await context.SaveChangesAsync();

                        product.MachineAttributes = (ICollection<MachineAttribute>)productAttributeLists;


                        context.Machines.Update(product);
                        await context.SaveChangesAsync();

                        await transaction.CommitAsync();

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        public async Task ChangeMachineThumbnail(MachineImage productImage)
        {
            using (var context = new MmrmsContext())
            {
                var oldMachineThumbnail = await context.MachineImages.FirstOrDefaultAsync(i => i.MachineId == productImage.MachineId && (bool)i.IsThumbnail);

                if (oldMachineThumbnail != null)
                {
                    context.MachineImages.Remove(oldMachineThumbnail);
                    await context.SaveChangesAsync();
                }

                DbSet<MachineImage> _dbSet = context.Set<MachineImage>();
                _dbSet.Add(productImage);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddMachineImages(int productId, List<MachineImage> newMachineImages)
        {
            using (var context = new MmrmsContext())
            {
                var oldNonThumbnailImages = context.MachineImages
                    .Where(i => i.MachineId == productId);

                context.MachineImages.RemoveRange(oldNonThumbnailImages);
                await context.SaveChangesAsync();

                context.MachineImages.AddRange(newMachineImages);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateMachineTerm(Machine product, List<MachineTerm> productTerms)
        {
            using (var context = new MmrmsContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        var oldTermMachines = context.MachineTerms
                       .Where(cp => cp.MachineId == product.MachineId);

                        context.MachineTerms.RemoveRange(oldTermMachines);
                        await context.SaveChangesAsync();

                        product.MachineTerms = productTerms;


                        context.Machines.Update(product);
                        await context.SaveChangesAsync();

                        await transaction.CommitAsync();

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        public async Task<Machine?> GetMachineWithSerialMachineNumberAndMachineImages(int productId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.Machines
                    .Include(p => p.MachineImages)
                    .Include(p => p.MachineSerialNumbers)
                    .FirstOrDefaultAsync(p => p.MachineId == productId);
            }
        }
    }
}
