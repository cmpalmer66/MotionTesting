using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.PlatformConfiguration;

// using Geolocation = Xamarin.Essentials.Geolocation;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;


namespace MotionTesting
{
    public partial class MainPage : ContentPage
    {
        private float _accelX;
        private float _accelY;
        private float _accelZ;

        private float _gyroX;
        private float _gyroY;
        private float _gyroZ;
        private double _latitude;
        private double _longitude;

        // private Geolocator Geolocator { get; set; }
        // private readonly ILocationService locationService;
        // private readonly ILocationServiceSettings locationServiceSettings;

        public MainPage()
        {
            Accelerometer.ReadingChanged += AccelerometerOnReadingChanged;
            Accelerometer.Start(SensorSpeed.Default);

            Gyroscope.ReadingChanged += GyroscopeOnReadingChanged;
            Gyroscope.Start(SensorSpeed.Default);

            Compass.ReadingChanged += CompassOnReadingChanged;
            Compass.Start(SensorSpeed.Default);

            Magnetometer.ReadingChanged += MagnetometerOnReadingChanged;
            Magnetometer.Start(SensorSpeed.Default);
            
            InitializeComponent();
            BindingContext = this;

            // this.locationService = SimpleIoc.Default.GetInstance<ILocationService>();
            // this.locationServiceSettings = SimpleIoc.Default.GetInstance<ILocationServiceSettings>();

            // Geolocator = new Geolocator();
            // Geolocator.StartCacheLocation();

            CrossGeolocator = new CrossGeolocator();
            CrossGeolocator.Current.PositionChanged += CurrentOnPositionChanged;
            CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 0.1, true);

            // GetCurrentLocation();
        }

        private double _magnetX;
        public string MagnetXS => $"{MagnetX:F2}";

        public double MagnetX
        {
            get => _magnetX;
            set
            {
                _magnetX = value;
                OnPropertyChanged("MagnetXS");
            }
        }

        private double _magnetY;
        public string MagnetYS => $"{MagnetY:F2}";

        public double MagnetY
        {
            get => _magnetY;
            set
            {
                _magnetY = value;
                OnPropertyChanged("MagnetYS");
            }
        }
        private double _magnetZ;
        public string MagnetZS => $"{MagnetZ:F2}";

        public double MagnetZ
        {
            get => _magnetZ;
            set
            {
                _magnetZ = value;
                OnPropertyChanged("MagnetZS");
            }
        }

        private void MagnetometerOnReadingChanged(object sender, MagnetometerChangedEventArgs e)
        {
            var reading = e.Reading.MagneticField;
            MagnetX = reading.X;
            MagnetY = reading.Y;
            MagnetZ = reading.Z;
        }

        private void CurrentOnPositionChanged(object sender, PositionEventArgs e)
        {
            Latitude = e.Position.Latitude;
            Longitude = e.Position.Longitude;
            Speed = e.Position.Speed;
            Course = e.Position.Heading;
            Accuracy = e.Position.Accuracy;
            Altitude = e.Position.Altitude;
            TimeStamp = e.Position.Timestamp;
            if (e.Position.IsFromMockProvider)
            {
                Console.WriteLine("From mock provider");
            }
        }

        private CrossGeolocator CrossGeolocator { get; set; }

        private double _headingMagneticNorth;
        public string HeadingMagneticNorthS => HeadingMagneticNorth.ToString();
        public double HeadingMagneticNorth
        {
            get => _headingMagneticNorth;
            set
            {
                _headingMagneticNorth = value;
                OnPropertyChanged("HeadingMagneticNorthS");
            }
        }

        private void CompassOnReadingChanged(object sender, CompassChangedEventArgs e)
        {
            HeadingMagneticNorth = e.Reading.HeadingMagneticNorth;
        }

        public string GyroXS => $"{GyroX:F2}";
        public string GyroYS => $"{GyroY:F2}";
        public string GyroZS => $"{GyroZ:F2}";

        private Location lastLocation { get; set; }

        public string LatitudeS => Latitude.ToString();

        public double Latitude
        {
            get => _latitude;
            set
            {
                _latitude = value;
                OnPropertyChanged("LatitudeS");
            }
        }

        public string LongitudeS => Longitude.ToString();

        public double Longitude
        {
            get => _longitude;
            set
            {
                _longitude = value;
                OnPropertyChanged("LongitudeS");
            }
        }

        private double _accuracy;
        public string AccuracyS => $"{Accuracy:F3}";

        public double Accuracy
        {
            get => _accuracy;
            set
            {
                _accuracy = value;
                OnPropertyChanged("AccuracyS");
            }
        }

        private double _course;
        public string CourseS => $"{Course:F1}";

        public double Course
        {
            get => _course;
            set
            {
                _course = value;
                OnPropertyChanged("CourseS");
            }
        }

        private double _speed;
        public string SpeedS => $"{Speed:F2}";

        public double Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                OnPropertyChanged("SpeedS");
            }
        }

        private double _altitude;
        public string AltitudeS => $"{Altitude:F2}";
        public double Altitude
        {
            get => _altitude;
            set
            {
                _altitude = value;
                OnPropertyChanged("AltitudeS");
            }
        }

        private DateTimeOffset _timestamp;
        public string TimeStampS => TimeStamp.ToString();
        public DateTimeOffset TimeStamp
        {
            get => _timestamp;
            set
            {
                _timestamp = value;
                OnPropertyChanged("TimeStampS");
            }
        }

        public async void GetCurrentLocation()
        {

            // BetterGeolocator:
            // var sw = Stopwatch.StartNew();
            //
            // // Retrieve current location
            // var location = await Geolocator.GetLocation(TimeSpan.FromSeconds(30), 200);
            //
            // if (location != null)
            // {
            //     Longitude = location.Coordinate.Longitude;
            //     Latitude = location.Coordinate.Latitude;
            //     Accuracy = location.Coordinate.Accuracy;
            //     Altitude = location.Coordinate.Altitude;
            //     Accuracy = location.Coordinate.Accuracy;
            //     TimeStamp = System.Convert.ToDateTime(location.UpdateDateTime);
            // }
            //
            // sw.Stop();
            // Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms");

            // Xamarin.Essentials:
            // GeolocationRequest req = new GeolocationRequest();
            // req.DesiredAccuracy = GeolocationAccuracy.Best;
            // req.Timeout = TimeSpan.FromSeconds(30);
            // lastLocation = await Geolocation.GetLocationAsync(req);
            // Longitude = lastLocation.Longitude;
            // Latitude = lastLocation.Latitude;
            // if (lastLocation.Accuracy != null) Accuracy = lastLocation.Accuracy.Value;
            // if (lastLocation.Course != null) Course = lastLocation.Course.Value;
            // if (lastLocation.Speed != null) Speed = lastLocation.Speed.Value;
            // if (lastLocation.Altitude != null) Altitude = lastLocation.Altitude.Value;
            // TimeStamp = lastLocation.Timestamp;
            // GetCurrentLocation();
        }

        private long count = 0;

        public float GyroX
        {
            get => _gyroX;
            set
            {
                // Console.WriteLine(value);
                // count++;
                // if (count % 20 == 0)
                // {
                //     count = 0;
                //     GetCurrentLocation();
                // }
                _gyroX = value;
                OnPropertyChanged("GyroXS");
            }
        }

        public float GyroY
        {
            get => _gyroY;
            set
            {
                // Console.WriteLine(value);
                _gyroY = value;
                OnPropertyChanged("GyroYS");
            }
        }

        public float GyroZ
        {
            get => _gyroZ;
            set
            {
                // Console.WriteLine(value);
                _gyroZ = value;
                OnPropertyChanged("GyroZS");
            }
        }

        private void GyroscopeOnReadingChanged(object sender, GyroscopeChangedEventArgs e)
        {
            GyroX = e.Reading.AngularVelocity.X;
            GyroY = e.Reading.AngularVelocity.Y;
            GyroZ = e.Reading.AngularVelocity.Z;
        }

        public string AccelXS => $"{AccelX:F2}";
        public string AccelYS => $"{AccelY:F2}";
        public string AccelZS => $"{AccelZ:F2}";

        public float AccelX
        {
            get => _accelX;
            set
            {
                // Console.WriteLine(value);
                _accelX = value;
                OnPropertyChanged("AccelXS");
            } 
        }

        public float AccelY
        {
            get => _accelY;
            set
            {
                // Console.WriteLine(value);
                _accelY = value;
                OnPropertyChanged("AccelYS");
            } 
        }
        public float AccelZ
        {
            get => _accelZ;
            set
            {
                // Console.WriteLine(value);
                _accelZ = value;
                OnPropertyChanged("AccelZS");
            } 
        }
        private void AccelerometerOnReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            AccelX = e.Reading.Acceleration.X;
            AccelY = e.Reading.Acceleration.Y;
            AccelZ = e.Reading.Acceleration.Z;
        }
    }
}
