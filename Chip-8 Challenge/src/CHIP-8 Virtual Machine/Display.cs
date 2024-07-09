namespace CHIP_8_Virtual_Machine;

public class Display
{
    private bool[,] _pixels;

    public bool this[int x, int y] 
    { 
        get => _pixels[x, y]; 
        set => _pixels[x, y] = value;
    }

    public Display()
    {
        _pixels = new bool[64, 32];
    }

    public bool DisplaySprite(int x, int y, byte[] spriteBytes)
    {
        bool pixelErased = false;
        for (int i = 0; i < spriteBytes.Length; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                byte spriteByte = spriteBytes[i];
                int localX = (x + j) % 64; // wraps the X coordinate (super-smart thinking from Garry)

                bool currentPixelState = _pixels[localX, y];
                bool shouldDisplay = (spriteByte & (0x1 << j)) != 0; // checks if bit j is set
                if (currentPixelState && shouldDisplay)
                {
                    pixelErased = true;
                    shouldDisplay = false;
                }

                _pixels[localX, y] = shouldDisplay;
            }
            
            y++;
        }

        return pixelErased;
    }

}
