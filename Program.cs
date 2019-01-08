using System;
namespace Savitsky
{
    
    class Program
    {
        static void Main(string[] args)
        {
            // inital condition provided by user

            double M = 10000.0;  // [kg]   Boats mass / load on water
            double LCG = 5;      // [m]    longitudinal center of gravity from transom
            double VCG = 0.5;    // [m]    Vertical center of gravity abobe keel (normal)
            double b = 3;        // [m]    Beam of the boat
            double beta = 20;    // [deg]  Deadrice of boat
            double eps = 0;      // [deg]  Inclination of thrust line relative to keel line
            double f = 0;        // [m]    Distance between Thrust line and COG ( Center of Gravity)
            double knt = 30;     // [knot] Speed of the boat
            double Np = 1;       // [-]    Number of engienes


            // Initialization of calculated variables
            double rad = Math.PI/180;        // Converts degrees to radians
            double V = knt * 0.514444444;    // Converts knop to m/s

            // Wate properties
            double rho = 1025.0;             // Density of salt water at 20deg Celcius
            double mu = 1.88e-3;             // Dynamic viscosity of salt water at 20 deg Celcius
            double nu = mu / rho;            // Kinemativ viscosity

            double g = 9.81;                 // Gravity constant


            double Cv = V / Math.Sqrt(g * b); // Beam Froude Number
            double Clb = M * g / (0.5 * V * V * b * b * rho);

            double aa = 0.0;
            double bb = 25.0;
            double Clo = aa;
            while ((bb - aa) >= 0.001)
            {
                // Find middle point 
                Clo = (aa + bb) / 2;

                // Check if middle  
                // point is root 
                double fc = Clo - 0.0065 * beta * Math.Pow(Clo, 0.6) - Clb;
                double fa = aa - 0.0065 * beta * Math.Pow(aa, 0.6) - Clb;
                if (fc == 0.0)
                    break;

                // Decide the side  
                // to repeat the steps 
                else if (fc * fa < 0)
                    bb = Clo;
                else
                    aa = Clo;
            }

            // Initial guess on ta,tb where this describe the initial trim angel, and TOL is the toleranse of Momentum solver
            double TOL = 0.0000001;
            double ta = 0.5; double tb = 12.0;

            // Decleration variables
            double Vm=0.0;
            double lambd = aa;
            // While loop to solve sum of forces = 0

            while ((tb - ta) >= TOL)
            {
                double tau = 0.5 * (ta + tb);

                // Solving nonlinear equation
                
                while ((bb - aa) >= 0.001)
                {
                    // Find middle point 
                    lambd = (aa + bb) / 2;

                    // Check if middle  
                    // point is root 
                    double fc1 = Math.Pow(tau,1.1) * (0.0120 * Math.Pow(lambd, 0.5) + 0.0055 *Math.Pow(lambd, 2.5) / Math.Pow(Cv,2)) - Clo;
                    double fa1 = Math.Pow(tau, 1.1) * (0.0120 * Math.Pow(aa, 0.5) + 0.0055 * Math.Pow(aa, 2.5) / Math.Pow(Cv, 2)) - Clo;
                    if (fc1 == 0.0)
                        break;

                    // Decide the side  
                    // to repeat the steps 
                    else if (fc1 * fa1 < 0)
                        bb = lambd;
                    else
                        aa = lambd;
                }

                Vm = V * Math.Pow((1 - (0.0120 * Math.Pow(lambd,0.5) * Math.Pow(tau,1.1) - 0.0065 * beta * Math.Pow((0.0120 * Math.Pow(lambd,0.5) * Math.Pow(tau,1.1)),0.6) / (lambd * Math.Cos(tau * rad)))), 0.5);


                tb = 0;ta = 0;




            }




            Console.WriteLine(Vm);
            Console.WriteLine(lambd);
            Console.ReadLine();




        }

        

    }
}
