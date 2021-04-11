namespace ExileCore.PoEMemory.Elements {
    public class DelveDarknessElement :Element { 
        public int Darkness { 
            get 
            { 
                var elem = GetChildFromIndices(0, 0, 2, 0);
                if (elem == null || (elem != null && elem.Text == null)) return 0;

                return int.Parse(elem.Text);
            } 
        }
    }
}
