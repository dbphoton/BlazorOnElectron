using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp1.Data
{
    public class WorkingTaskService
    {
        public async Task<List<WorkingTask>> GetWorkingTasksAsync()
        {
            await Task.Delay(5000);
            var x = Task.FromResult(Enumerable.Range(1, 5).Select(index => new WorkingTask
            {
                ProjectCode = $"CODE_{index}",
                TaskName = $"NAME_{index}",
                Id = index
            })).Result.ToList();

            return x;
        }
    }








}
