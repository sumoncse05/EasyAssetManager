using EasyAssetManagerCore.Model.CommonModel;
using System.Collections.Generic;

namespace EasyAssetManagerCore.BusinessLogic.Common
{
    public interface IBaseService<T>
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        Message Insert(T t, AppSession session);
        Message Delete(T id, AppSession session);
    }
}
