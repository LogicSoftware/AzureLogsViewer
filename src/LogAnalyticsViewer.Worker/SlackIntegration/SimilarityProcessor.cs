using F23.StringSimilarity;
using LogAnalyticsViewer.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogAnalyticsViewer.Worker.SlackIntegration
{
    public class SimilarityProcessor : Cosine
    {
        private double _similarity;

        public SimilarityProcessor(double similarity = 0.97, int k = 4) : base(k)
        {
            _similarity = similarity;
        }

        private EventForSlack Profile(Event e)
        {
            var res = new EventForSlack(e);
            res.Profile = GetProfile(res.Message);

            return res;
        }

        private bool AreSimilar(EventForSlack e1, EventForSlack e2) => Similarity(e1.Profile, e2.Profile) > _similarity;

        public List<EventForSlack> GetUniqueWithTotal(List<Event> events)
        {            
            var result = new List<EventForSlack>();
            
            foreach (var e in events)
            {
                var profiled = Profile(e);

                var recorded = result.FirstOrDefault(res => AreSimilar(profiled, res));
                if (recorded == null)
                {
                    result.Add(profiled);
                }
                else
                {
                    recorded.Total++;
                }
            }

            return result;
        }
    }
}
