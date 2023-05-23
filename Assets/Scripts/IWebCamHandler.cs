using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Granden.BeautyWithShader
{
    public interface IWebCamHandler
    {
        // Start is called before the first frame update
        bool Initial(RawImage CamImg, TMP_Dropdown Dropdown);

        // Update is called once per frame
        WebCamTexture GetWebCamTexture();

    }
}
