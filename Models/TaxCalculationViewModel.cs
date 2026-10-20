using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Globalization;

namespace SusanOsuolaleCV.Models
{
    public class TaxCalculationViewModel
    {
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid income amount.")]
        public decimal AnnualIncome { get; set; }

        public decimal EstimatedTax { get; set; }
        public decimal NetIncome { get; set; }

        // User-facing notice when income has no tax or other messages
        public bool IsTaxable { get; set; }
        public string Notice { get; set; } = string.Empty;

        // For 2026, the Nigeria Tax Act 2025 uses these progressive personal income-tax bands:
        public List<TaxBracket> Brackets { get; } = new List<TaxBracket>
        {
            new TaxBracket { Threshold = 800000m, Rate = 0.00m },
            new TaxBracket { Threshold = 3000000m, Rate = 0.15m },
            new TaxBracket { Threshold = 12000000m, Rate = 0.18m },
            new TaxBracket { Threshold = 25000000m, Rate = 0.21m },
            new TaxBracket { Threshold = 50000000m, Rate = 0.23m },
            new TaxBracket { Threshold = decimal.MaxValue, Rate = 0.25m }
        };

        public void Calculate()
        {
            // Basic progressive calculation for demonstration.
            var income = AnnualIncome;
            decimal tax = 0m;
            decimal previous = 0m;

            foreach (var b in Brackets)
            {
                var cap = b.Threshold;
                var taxable = (income > cap ? cap - previous : income - previous);
                if (taxable <= 0) break;
                tax += taxable * b.Rate;
                previous = cap;
            }

            EstimatedTax = tax;
            NetIncome = AnnualIncome - EstimatedTax;

            // Provide a clear notice when no tax applies
            if (EstimatedTax <= 0m)
            {
                IsTaxable = false;
                if (AnnualIncome <= 0m)
                {
                    Notice = "Please enter an income amount to calculate tax.";
                }
                else
                {
                    Notice = "No tax is due for this income. You are below the taxable threshold.";
                }
            }
            else
            {
                IsTaxable = true;
                Notice = string.Empty;
            }
        }

        public string FormatNaira(decimal amount)
        {
            // Use plain ASCII currency label to avoid font rendering issues for the Naira symbol
            return "NGN " + amount.ToString("N", CultureInfo.GetCultureInfo("en-NG"));
        }
    }

    public class TaxBracket
    {
        public decimal Threshold { get; set; }
        public decimal Rate { get; set; }
    }
}
