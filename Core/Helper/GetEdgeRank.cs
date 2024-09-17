using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Core.Helper
{
    public class GetEdgeRankAlo
    {
        public static double GetEdgeRank(int ReactCount, int CommentCount, int ShareCount, DateTime CreateAt)
        {
            double timeDecayFactor = Math.Exp(-(DateTime.Now - CreateAt).TotalHours / 0.8); // Hệ số suy giảm theo thời gian
            int interactionScore = ReactCount * 3 + CommentCount + ShareCount;


            double newPostWeight = 1.0;
            if ((DateTime.Now - CreateAt).TotalHours <= 1)
            {
                newPostWeight = 2.0; 
                interactionScore = interactionScore == 0 ? 1 : interactionScore;
            }

            return timeDecayFactor * interactionScore * newPostWeight;
        }
    }
}
