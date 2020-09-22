using ExemploAlugaCarroServicoSemInterface.Entities;
using System;

namespace ExemploAlugaCarroServicoSemInterface.Services
{
    class RentalService
    {
        //quando se coloca o private, é pq o dado somente pode ser obtidos e não alterados.
        public double PricePerHour { get; private set; }
        public double PricePerDay { get; private set; }

        //fazendo de uma forma inadequada
        private BrazilTaxService _brazilTaxService = new BrazilTaxService();

        public RentalService(double pricePerHour, double pricePerDay)
        {
            PricePerHour = pricePerHour;
            PricePerDay = pricePerDay;
        }

        public void ProcessInvoice(CarRental carRental)
        {
            //calculo para pegar a duração de uma data para a outra
            TimeSpan duration = carRental.Finish.Subtract(carRental.Start);

            double basicPayment = 0.0;

            //confiro se a duração é menor que 12 horas.
            if (duration.TotalHours <= 12.0)
            {
                basicPayment = PricePerHour * Math.Ceiling(duration.TotalHours); //Math.ceiling é arredondamento pra cima
            }
            else
            {
                basicPayment = PricePerDay * Math.Ceiling(duration.TotalDays); 
            }

            //O serviço de imposto está calculando o imposto baseado no pagamento básico que foi gerado na regra acima.
            double tax = _brazilTaxService.Tax(basicPayment);

            carRental.Invoice = new Invoice(basicPayment, tax);
        }
    }
}
