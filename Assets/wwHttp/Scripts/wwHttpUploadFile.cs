using UnityEngine;
using System.Collections;
using System.IO;

public class wwHttpUploadFile : wwHttpClient {
    public override WWW CreateWWW()
    {
        WWWForm form = new WWWForm();
        if (string.IsNullOrEmpty(httpInfo.uploadFile))
        {
            byte[] b = ReadFile(httpInfo.uploadFile);
            FileInfo f = new FileInfo(httpInfo.uploadFile);
            form.AddBinaryData(f.Name, b, f.Name);
        }
        WWW www = wwHttp.WWWPost(httpInfo.url, form);
        httpInfo.www = www;
        return www;
    }

    byte[] ReadFile(string path)
    {
        if (!File.Exists(path))
        {
            wwDebug.LogWarning(string.Format("File:{0} not exist!",path));
            return null;
        }
        FileInfo fi = new FileInfo(path);
        long len = fi.Length;
        FileStream fs = new FileStream(path, FileMode.Open);
        byte[] buffer = new byte[len];
        fs.Read(buffer, 0, (int)len);
        fs.Close();
        return buffer;
    }
}
