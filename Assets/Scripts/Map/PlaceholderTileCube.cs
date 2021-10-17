using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder
{

    public class PlaceholderTileCube : MonoBehaviour
    {
        public delegate void TileCubeEvent(PlaceholderTileCube placeholderTileCube);
        public TileCubeEvent onMouseOver;
        public TileCubeEvent onMouseOut;

        void OnMouseOver()
        {
            if (onMouseOver != null)
            {
                onMouseOver(this);
            }
        }

        void OnMouseOut()
        {
            if (onMouseOut != null)
            {
                onMouseOut(this);
            }
        }
    }
}
