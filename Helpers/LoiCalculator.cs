using System;
using System.Collections.Generic;
using System.Linq;
using best_hackathon_2025.MongoDB.Collections;

namespace best_hackathon_2025.Helpers
{
    public class LoiCalculator
    {
        private readonly Dictionary<string, List<string>> _disabilityCategories = new()
        {
            { "vision", new List<string> { "Тактильна плитка", "Звукові сигнали", "Дублювання тексту шрифтом Брайля", "Вказівники для слабозорих", "Звукові оголошення" } },
            { "hearing", new List<string> { "Дублювання інформації жестовою мовою", "Візуальні вказівники" } },
            { "movement", new List<string> { "Пандус", "Ліфт", "Занижений бордюр", "Доступний вхід без сходів", "Широкі проходи для колясок", "Інклюзивна вбиральня", "Пандус в транспорті", "Паркування для інвалідів" } },
            { "prosthesis", new List<string> { "Антиковзаюче покриття", "Доступний вхід без сходів", "Широкі проходи для колясок" } },
            { "coordination", new List<string> { "Антиковзаюче покриття", "Доступний вхід без сходів", "Розмітка на підлозі" } }
        };

        public double CalculateLoi(Point point, string? disabilityType = null)
        {
            if (disabilityType == null)
            {
                return Math.Round((double)point.Categories.Count / 20 * 10, 2);
            }

            if (!_disabilityCategories.ContainsKey(disabilityType))
                return 0;

            var targetCategories = _disabilityCategories[disabilityType];
            int matched = point.Categories.Count(c => targetCategories.Contains(c));

            return Math.Round((double)matched / targetCategories.Count * 10, 2);
        }
    }
}
