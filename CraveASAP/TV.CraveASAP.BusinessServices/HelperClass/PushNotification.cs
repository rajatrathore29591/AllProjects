using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using PushSharp;
using PushSharp.Core;
using PushSharp.Android;
using PushSharp.Apple;
using System.Web.Hosting;
using System.Web;
using TV.CraveASAP.BusinessEntities;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.Net.Sockets;
using System.Net;

namespace TV.CraveASAP.BusinessServices.HelperClass
{

    public class PushNotification
    {
        //String CertificateName = "EddeeVendorPushP12.p12";
        //String CertificatePwd = "123456";
        String FriendName = "Apple Production IOS Push Services: com.tv.EddeeVendorApp";
        String ProductionKeyFriendName = "Production";
        SslStream sslStream;
        // JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        static void DeviceSubscriptionChanged(object sender,
          string oldSubscriptionId, string newSubscriptionId, INotification notification)
        {
            //Do something here
        }

        //this even raised when a notification is successfully sent
        static void NotificationSent(object sender, INotification notification)
        {
            //Do something here
        }

        //this is raised when a notification is failed due to some reason
        static void NotificationFailed(object sender,
        INotification notification, Exception notificationFailureException)
        {
            //Do something here
        }

        //this is fired when there is exception is raised by the channel
        static void ChannelException
            (object sender, IPushChannel channel, Exception exception)
        {
            //Do something here
        }

        //this is fired when there is exception is raised by the service
        static void ServiceException(object sender, Exception exception)
        {
            //Do something here
        }

        //this is raised when the particular device subscription is expired
        static void DeviceSubscriptionExpired(object sender,
        string expiredDeviceSubscriptionId,
            DateTime timestamp, INotification notification)
        {
            //Do something here
        }

        //this is raised when the channel is destroyed
        static void ChannelDestroyed(object sender)
        {
            //Do something here
        }

        //this is raised when the channel is created
        static void ChannelCreated(object sender, IPushChannel pushChannel)
        {
            //Do something here
        }

        public void SendPushNotification(List<DeviceEntity> Device)
        {
            foreach (DeviceEntity row in Device)
            {
                string[] splittedToken = row.Device.Split(','); string[] splittedPlatform = row.DeviceType.Split(',');
                int i = 0;
                foreach (var dt in splittedToken)
                {
                    var push = new PushBroker();
                    //Wire up the events for all the services that the broker registers
                    push.OnNotificationSent += NotificationSent;
                    push.OnChannelException += ChannelException;
                    push.OnServiceException += ServiceException;
                    push.OnNotificationFailed += NotificationFailed;
                    push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
                    push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
                    push.OnChannelCreated += ChannelCreated;
                    push.OnChannelDestroyed += ChannelDestroyed;

                    if (splittedToken[i] != "simulator" && splittedToken[i].Length > 30

                        )
                        if (splittedPlatform[i] == "ios")
                        {
                            try
                            {

                                PushMessage(row.MessageEnglish, row.MessageThai, splittedToken[i], row.badge, "", row.AppType, row.type, row.vendorId, row.subCategory, row.optCategory);

                            }
                            catch
                            {
                                continue;
                            }
                        }
                        else if (splittedPlatform[i] == "android")
                        {
                            try
                            {
                                //---------------------------
                                // ANDROID GCM NOTIFICATIONS
                                //---------------------------
                                //Configure and start Android GCM
                                //IMPORTANT: The API KEY comes from your Google APIs Console App, 
                                //under the API Access section, 
                                //  by choosing 'Create new Server key...'
                                //  You must ensure the 'Google Cloud Messaging for Android' service is 
                                //enabled in your APIs Console
                                if (row.AppType == "User")
                                {
                                    push.RegisterGcmService(new GcmPushChannelSettings("AIzaSyBEhdIP2fnfyg3HPWLMNudhRRtaQ9GdF2A"));
                                }
                                else
                                {
                                    push.RegisterGcmService(new GcmPushChannelSettings("AIzaSyB1Frr3M23HvNtKH6BseRuCRFugYds0xNs"));

                                }
                                var javaScriptSerializer = new
                                System.Web.Script.Serialization.JavaScriptSerializer();
                                PushNotificationsResponse pushNotif = new PushNotificationsResponse();
                                pushNotif.alert = row.Alert;
                                pushNotif.message = row.MessageEnglish;
                                pushNotif.messageThai = row.MessageThai;
                                pushNotif.badge = Convert.ToString(row.badge);
                                pushNotif.sound = "sound.caf";
                                pushNotif.type = row.type;
                                pushNotif.vendorId = row.vendorId;
                                pushNotif.subCategory = row.subCategory;
                                pushNotif.optCategory = row.optCategory;

                                string jsonString = javaScriptSerializer.Serialize(pushNotif);

                                push.QueueNotification(new GcmNotification().ForDeviceRegistrationId(DeviceToken(splittedToken[i])).WithJson(jsonString));
                                push.StopAllServices(waitForQueuesToFinish: true);
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    i = i + 1;
                }
            }
        }
        protected void PushMessage(string MessageEnglish, string MessageThai, string DeviceToken, int Badge, string Custom_Field, string AppType, 
            string type, string vendorId, string subCategory, string optCategory)
        {
            // DeviceToken = "f5dbcc3f22c634aaf68392dc21be902c5f6cd59ca71774ea6fbc4adbe6a24537";//  iphone device token
            int port = 2195;
            //String hostname = "gateway.sandbox.push.apple.com";
            String hostname = "gateway.push.apple.com";
            string P12_file = string.Empty;
            if (HttpContext.Current != null)
            { P12_file = HttpContext.Current.Server.MapPath(""); }
            else { P12_file = AppDomain.CurrentDomain.BaseDirectory; }
            string certificatePath = P12_file + @"\EddeeVendorPushP12.p12";

            if (AppType == "Vendor")
            {
                certificatePath = P12_file + @"\EddeeVendorPushP12.p12";
            }
            else
            {
                certificatePath = P12_file + @"\Eddeeuser_dis.p12";
            }

            string certificatePassword = "123456";

            X509Certificate2 clientCertificate = new X509Certificate2(certificatePath, certificatePassword, X509KeyStorageFlags.MachineKeySet);
            X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);

            TcpClient client = new TcpClient(hostname, port);
            SslStream sslStream = new SslStream(
                            client.GetStream(),
                            false,
                            new RemoteCertificateValidationCallback(ValidateServerCertificate),
                            null
            );

            try
            {
                sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Default, false);
            }
            catch (AuthenticationException ex)
            {
                Console.WriteLine("Authentication failed");
                client.Close();
                //  Request.SaveAs(Microsoft.SqlServer.Server.MapPath("Authenticationfailed.txt"), true);
                return;
            }


            //// Encode a test message into a byte array.
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);

            writer.Write((byte)0);  //The command
            writer.Write((byte)0);  //The first byte of the deviceId length (big-endian first byte)
            writer.Write((byte)32); //The deviceId length (big-endian second byte)

            byte[] b0 = HexString2Bytes(DeviceToken);
            WriteMultiLineByteArray(b0);

            writer.Write(b0);
            String payload;
            string strmsgbody = "";

            strmsgbody = "Hey Eddee!";
         //   language = "English";
            //  Debug.WriteLine("during testing via device!");
            //   Request.SaveAs(Server.MapPath("APNSduringdevice.txt"), true); 

            payload = "{\"aps\":{\"alert\":\"" + MessageEnglish + "\",\"alertThai\":\"" + MessageThai + "\",\"badge\":" + Convert.ToString(Badge) + ",\"sound\":\"mailsent.wav\"},\"type\":\"" + type + "\",\"vendorId\":\"" + vendorId + "\",\"subCategory\":\"" + subCategory + "\",\"optCategory\":\"" + optCategory + "\"}";

            writer.Write((byte)0); //First byte of payload length; (big-endian first byte)
            writer.Write((byte)payload.Length);     //payload length (big-endian second byte)

            byte[] b1 = System.Text.Encoding.UTF8.GetBytes(payload);
            writer.Write(b1);
            writer.Flush();

            byte[] array = memoryStream.ToArray();
            //     Debug.WriteLine("This is being sent...\n\n");
            //   Debug.WriteLine(array);
            try
            {
                sslStream.Write(array);
                sslStream.Flush();
            }
            catch
            {
                //    Debug.WriteLine("Write failed buddy!!");
                //   Request.SaveAs(Server.MapPath("Writefailed.txt"), true);
            }

            client.Close();
            //   Debug.WriteLine("Client closed.");
            //   Request.SaveAs(Server.MapPath("APNSSuccess.txt"), true);
        }
        private byte[] HexString2Bytes(string hexString)
        {
            //check for null
            if (hexString == null) return null;
            //get length
            int len = hexString.Length;
            if (len % 2 == 1) return null;
            int len_half = len / 2;
            //create a byte array
            byte[] bs = new byte[len_half];
            try
            {
                //convert the hexstring to bytes
                for (int i = 0; i != len_half; i++)
                {
                    bs[i] = (byte)Int32.Parse(hexString.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Exception : " + ex.Message);
            }
            //return the byte array
            return bs;
        }
        // The following method is invoked by the RemoteCertificateValidationDelegate.
        public static bool ValidateServerCertificate(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }
        public static void WriteMultiLineByteArray(byte[] bytes)
        {
            const int rowSize = 20;
            int iter;

            Console.WriteLine("initial byte array");
            Console.WriteLine("------------------");

            for (iter = 0; iter < bytes.Length - rowSize; iter += rowSize)
            {
                Console.Write(
                    BitConverter.ToString(bytes, iter, rowSize));
                Console.WriteLine("-");
            }

            Console.WriteLine(BitConverter.ToString(bytes, iter));
            Console.WriteLine();
        }
        public string DeviceToken(string device)
        {
            string token = string.Empty;
            string[] splittedToken = device.Split(',');

            if (splittedToken.Length > 0)
            {
                token = Convert.ToString(splittedToken.Last());

            }
            else
            {
                token = splittedToken[0];
            }

            return token;
        }
    }
}

