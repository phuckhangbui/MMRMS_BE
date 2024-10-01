using BusinessObject;

namespace DAO
{
    public class EmployeeTaskLogDao : BaseDao<TaskLog>
    {
        private static EmployeeTaskLogDao instance = null;
        private static readonly object instacelock = new object();

        private EmployeeTaskLogDao()
        {

        }

        public static EmployeeTaskLogDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EmployeeTaskLogDao();
                }
                return instance;
            }
        }
    }
}
