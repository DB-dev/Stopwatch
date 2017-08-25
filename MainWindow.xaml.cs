using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace clock
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /**
         * Stopwatch
         * 
         * DB-dev
         * 
         */

       
        public static DateTime hora_s;  // Hora guardada 
        public static DateTime hora_u;  // Hora ultima guardada 

        public static TimeSpan hora_a;  // Acumulado de horas
        public static TimeSpan res;     // Diferencia de horas

        public static Timer t_set;      // Para refrescar el reloj

        public MainWindow()
        {
            InitializeComponent();

            Inicializar();
        }

        // Esta función será como mi constructor para ciertas variables.
        private void Inicializar()
        {
            // Inicializamos el timer el evento de actualización del reloj.
            t_set = new Timer(10);
            t_set.Elapsed += new ElapsedEventHandler(timerdelReloj);

            // Inicializamos el Acumulado de horas
            hora_a = new TimeSpan(0, 0, 0);

            b_continue.IsEnabled = false;
        }

        // Cuando pulsamos Start arrancamos el timer del reloj
        private void b_start_Click(object sender, RoutedEventArgs e)
        {
            // Grabamos la hora de cuando se le dio al Start
            hora_s = DateTime.Now;

            // Reiniciamos el acumulado
            hora_a = new TimeSpan(0, 0, 0);

            t_set.Enabled = true;

            b_continue.IsEnabled = false;
        }

        // Cuando pulsamos Continue el contador sigue
        private void b_continue_Click(object sender, RoutedEventArgs e)
        {
            if (!t_set.Enabled)
            {
                hora_a = hora_a.Add(res);

                hora_s = DateTime.Now;

                t_set.Enabled = true;

                b_continue.IsEnabled = false;
            }
        }

        // Cuando pulsamos reset reiniciamos todo y lo paramos a cero
        private void b_reset_Click(object sender, RoutedEventArgs e)
        {
            t_set.Enabled = false;

            // Reiniciamos el acumulado
            hora_a = new TimeSpan(0, 0, 0);

            t_clock.Text = "00:00:00:00";
            b_continue.IsEnabled = false;
        }

        // Cuando pulsamos Stop paramos el timer del reloj
        private void b_stop_Click(object sender, RoutedEventArgs e)
        {
            if (t_set.Enabled)
            {
                t_set.Enabled = false;

                b_continue.IsEnabled = true;
            }
        }

        // Logica tras el cada tick del timer
        private void timerdelReloj(Object source, System.Timers.ElapsedEventArgs e)
        {
            hora_u = DateTime.Now;

            // Para el crono, restamos la hora actual con de cuando se le pulsó al start
            res = hora_u.Subtract(hora_s);

            TimeSpan tiempoAux = res.Add(hora_a);

            // Los TimeSpan son tan solo una hora, pero no da muchas opciones a la hora de formato, lo sumamos a una fecha referencia y lo usamos para formato
            DateTime referencia = new DateTime(2017, 1, 1);
            referencia += tiempoAux;
            
            String aux = referencia.ToString("HH:mm:ss:ff");

            // Al parecer en WPF los elementos de la UI solo se pueden acceder desde los thread de la UI 
            // y hay que hacer uso de Dispatch para poder hacer lo que sea sobre esos elementos.
            t_clock.Dispatcher.BeginInvoke(DispatcherPriority.Normal, 
                (Action)(() =>
                {
                    t_clock.Text = aux;
                })
            );
        }

    }
}
