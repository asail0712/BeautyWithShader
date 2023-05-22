using System;

namespace Granden.Common
{
    public interface IButton
    {
        event Action OnClick;
    }
}