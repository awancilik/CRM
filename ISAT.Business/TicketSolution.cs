using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAT.Business
{
    public class TicketSolution
    {
        public string HowToSolving { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class TicketSolutionList
    {
        public List<TicketSolution> Solutions { get; set; }
        public int Count { get; set; }

        public void Add(TicketSolution solution)
        {
            Solutions.Add(solution);
            Count++;
        }

        public void Remove(TicketSolution solution)
        {
            Solutions.Remove(solution);
            Count--;
        }

        public TicketSolutionList()
        {
            Solutions = new List<TicketSolution>();
        }
    }
}
