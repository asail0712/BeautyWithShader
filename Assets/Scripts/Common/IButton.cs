using System;

namespace Granden
{
    public interface IButton
    {
        event Action OnClick;
    }
}