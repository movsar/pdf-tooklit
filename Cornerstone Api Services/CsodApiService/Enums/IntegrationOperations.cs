using System;
using System.Collections.Generic;
using System.Text;

namespace CornerstoneApiServices.Enums
{
    public enum IntegrationOperations
    {
        locationCleanup,    // Cleanup location hierarchy (starts internal polling process)
        encryptClientData,  // Encrypt all xml files in the ClientData folder
        decryptClientData,  // Decrypt all xml files in the ClientData folder
        pollLocation,       // Check location OU for Parent ID
        connectionTest      // Tests REST and SOAP connection using GetOUs and EchoUsers
    }
}
