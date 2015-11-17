using System.Drawing;
using System.Drawing.Imaging;

namespace VncSharp
{
    public class VncClientWithImage : VncClient
    {
        public Bitmap Frame { get; set; }

        public event VncImageUpdated ImageUpdated;

        public override void Initialize()
        {
            base.Initialize();

            Frame = new Bitmap(Framebuffer.Width, Framebuffer.Height, PixelFormat.Format32bppArgb);
        }

        protected override void ProcessFrameBufferUpdate()
        {
            var rectangles = rfb.ReadFramebufferUpdate();

            if (CheckIfThreadDone()) return;

            for (var i = 0; i < rectangles; ++i)
            {
                // Get the update rectangle's info
                Rectangle rectangle;
                int enc;
                rfb.ReadFramebufferUpdateRectHeader(out rectangle, out enc);

                // Build a derived EncodedRectangle type and pull-down all the pixel info
                var er = factory.Build(rectangle, enc);
                er.Decode();

                er.Order = i;

                er.Draw(Frame);
            }

            // Let the UI know that an updated rectangle is available, but check
            // to see if the user closed things down first.
            if (!CheckIfThreadDone())
            {
                OnImageUpdated(Frame);
            }
        }

        protected virtual void OnImageUpdated(Bitmap latestframe)
        {
            // Maybe copy the image?  Ideally send out the bytes of the image?

            var handler = ImageUpdated;
            if (handler != null) handler(latestframe);
        }
    }
}