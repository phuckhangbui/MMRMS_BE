﻿using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class EmployeeTaskDao : BaseDao<EmployeeTask>
    {
        private static EmployeeTaskDao instance = null;
        private static readonly object instacelock = new object();

        private EmployeeTaskDao()
        {

        }

        public static EmployeeTaskDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EmployeeTaskDao();
                }
                return instance;
            }
        }

        public async Task<IEnumerable<EmployeeTask>> GetEmployeeTasks()
        {
            using (var context = new MmrmsContext())
            {
                return await context.EmployeeTasks.Include(d => d.Staff).Include(d => d.Manager).OrderByDescending(p => p.DateCreate).ToListAsync();
            }
        }

        public async Task<EmployeeTask> GetEmployeeTask(int employeeTaskId)
        {
            using (var context = new MmrmsContext())
            {
                return await context.EmployeeTasks.Include(d => d.Staff).Include(d => d.Manager).FirstOrDefaultAsync(d => d.EmployeeTaskId == employeeTaskId);
            }
        }
    }
}
