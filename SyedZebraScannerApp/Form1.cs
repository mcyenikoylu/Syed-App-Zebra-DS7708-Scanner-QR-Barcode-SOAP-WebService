using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;
using System.Diagnostics;
using System.IO;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using CoreScanner;
using System.Drawing.Printing;
using DevExpress.XtraBars.Ribbon.ViewInfo;
using DevExpress.XtraPrinting.BarCode;
using DevExpress.XtraReports.UI;
using System.Threading;
using DevExpress.XtraPrinting;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Globalization;
using System.Configuration;
using System.Xml;

namespace SyedZebraScannerApp
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        CCoreScannerClass m_pCoreScanner;
        bool m_bSuccessOpen;//Is open success
        Scanner[] m_arScanners;
        XmlReader m_xml;
        int m_nTotalScanners;

        #region Symbology types
        // Symbology types 


        const int ST_NOT_APP = 0x00;
        const int ST_CODE_39 = 0x01;
        const int ST_CODABAR = 0x02;
        const int ST_CODE_128 = 0x03;
        const int ST_D2OF5 = 0x04;
        const int ST_IATA = 0x05;
        const int ST_I2OF5 = 0x06;
        const int ST_CODE93 = 0x07;
        const int ST_UPCA = 0x08;
        const int ST_UPCE0 = 0x09;
        const int ST_EAN8 = 0x0a;
        const int ST_EAN13 = 0x0b;
        const int ST_CODE11 = 0x0c;
        const int ST_CODE49 = 0x0d;
        const int ST_MSI = 0x0e;
        const int ST_EAN128 = 0x0f;
        const int ST_UPCE1 = 0x10;
        const int ST_PDF417 = 0x11;
        const int ST_CODE16K = 0x12;
        const int ST_C39FULL = 0x13;
        const int ST_UPCD = 0x14;
        const int ST_TRIOPTIC = 0x15;
        const int ST_BOOKLAND = 0x16;
        const int ST_UPCA_W_CODE128 = 0x17; // For UPC-A w/Code 128 Supplemental
        const int ST_JAN13_W_CODE128 = 0x78; // For EAN/JAN-13 w/Code 128 Supplemental
        const int ST_NW7 = 0x18;
        const int ST_ISBT128 = 0x19;
        const int ST_MICRO_PDF = 0x1a;
        const int ST_DATAMATRIX = 0x1b;
        const int ST_QR_CODE = 0x1c;
        const int ST_MICRO_PDF_CCA = 0x1d;
        const int ST_POSTNET_US = 0x1e;
        const int ST_PLANET_CODE = 0x1f;
        const int ST_CODE_32 = 0x20;
        const int ST_ISBT128_CON = 0x21;
        const int ST_JAPAN_POSTAL = 0x22;
        const int ST_AUS_POSTAL = 0x23;
        const int ST_DUTCH_POSTAL = 0x24;
        const int ST_MAXICODE = 0x25;
        const int ST_CANADIN_POSTAL = 0x26;
        const int ST_UK_POSTAL = 0x27;
        const int ST_MACRO_PDF = 0x28;
        const int ST_MACRO_QR_CODE = 0x29;
        const int ST_MICRO_QR_CODE = 0x2c;
        const int ST_AZTEC = 0x2d;
        const int ST_AZTEC_RUNE = 0x2e;
        const int ST_DISTANCE = 0x2f;
        const int ST_GS1_DATABAR = 0x30;
        const int ST_GS1_DATABAR_LIMITED = 0x31;
        const int ST_GS1_DATABAR_EXPANDED = 0x32;
        const int ST_PARAMETER = 0x33;
        const int ST_USPS_4CB = 0x34;
        const int ST_UPU_FICS_POSTAL = 0x35;
        const int ST_ISSN = 0x36;
        const int ST_SCANLET = 0x37;
        const int ST_CUECODE = 0x38;
        const int ST_MATRIX2OF5 = 0x39;
        const int ST_UPCA_2 = 0x48;
        const int ST_UPCE0_2 = 0x49;
        const int ST_EAN8_2 = 0x4a;
        const int ST_EAN13_2 = 0x4b;
        const int ST_UPCE1_2 = 0x50;
        const int ST_CCA_EAN128 = 0x51;
        const int ST_CCA_EAN13 = 0x52;
        const int ST_CCA_EAN8 = 0x53;
        const int ST_CCA_RSS_EXPANDED = 0x54;
        const int ST_CCA_RSS_LIMITED = 0x55;
        const int ST_CCA_RSS14 = 0x56;
        const int ST_CCA_UPCA = 0x57;
        const int ST_CCA_UPCE = 0x58;
        const int ST_CCC_EAN128 = 0x59;
        const int ST_TLC39 = 0x5A;
        const int ST_CCB_EAN128 = 0x61;
        const int ST_CCB_EAN13 = 0x62;
        const int ST_CCB_EAN8 = 0x63;
        const int ST_CCB_RSS_EXPANDED = 0x64;
        const int ST_CCB_RSS_LIMITED = 0x65;
        const int ST_CCB_RSS14 = 0x66;
        const int ST_CCB_UPCA = 0x67;
        const int ST_CCB_UPCE = 0x68;
        const int ST_SIGNATURE_CAPTURE = 0x69;
        const int ST_MOA = 0x6A;
        const int ST_PDF417_PARAMETER = 0x70;
        const int ST_CHINESE2OF5 = 0x72;
        const int ST_KOREAN_3_OF_5 = 0x73;
        const int ST_DATAMATRIX_PARAM = 0x74;
        const int ST_CODE_Z = 0x75;
        const int ST_UPCA_5 = 0x88;
        const int ST_UPCE0_5 = 0x89;
        const int ST_EAN8_5 = 0x8a;
        const int ST_EAN13_5 = 0x8b;
        const int ST_UPCE1_5 = 0x90;
        const int ST_MACRO_MICRO_PDF = 0x9A;
        const int ST_OCRB = 0xA0;
        const int ST_OCRA = 0xA1;
        const int ST_PARSED_DRIVER_LICENSE = 0xB1;
        const int ST_PARSED_UID = 0xB2;
        const int ST_PARSED_NDC = 0xB3;
        const int ST_DATABAR_COUPON = 0xB4;
        const int ST_PARSED_XML = 0xB6;
        const int ST_HAN_XIN_CODE = 0xB7;
        const int ST_CALIBRATION = 0xC0;
        const int ST_GS1_DATAMATRIX = 0xC1;
        const int ST_GS1_QR = 0xC2;
        const int BT_MAINMARK = 0xC3;
        const int BT_DOTCODE = 0xC4;
        const int BT_GRID_MATRIX = 0xC8;
        const int BT_UDI_CODE = 0xCC;
        const int ST_EPC_RAW = 0xE0;


        //End Symbology Types

        #endregion

        #region CORESCANNER PROTOCOL
        //****** CORESCANNER PROTOCOL ******//
        const int GET_VERSION = 1000;
        const int REGISTER_FOR_EVENTS = 1001;
        const int UNREGISTER_FOR_EVENTS = 1002;
        const int GET_PAIRING_BARCODE = 1005; // Get  Blue tooth scanner pairing bar code
        const int CLAIM_DEVICE = 1500;
        const int RELEASE_DEVICE = 1501;
        const int ABORT_MACROPDF = 2000;
        const int ABORT_UPDATE_FIRMWARE = 2001;
        const int DEVICE_AIM_OFF = 2002;
        const int DEVICE_AIM_ON = 2003;
        const int FLUSH_MACROPDF = 2005;
        const int GET_ALL_PARAMETERS = 2006;
        const int GET_PARAMETERS = 2007;
        const int DEVICE_GET_SCANNER_CAPABILITIES = 2008;
        const int DEVICE_LED_OFF = 2009;
        const int DEVICE_LED_ON = 2010;
        const int DEVICE_PULL_TRIGGER = 2011;
        const int DEVICE_RELEASE_TRIGGER = 2012;
        const int DEVICE_SCAN_DISABLE = 2013;
        const int DEVICE_SCAN_ENABLE = 2014;
        const int SET_PARAMETER_DEFAULTS = 2015;
        const int DEVICE_SET_PARAMETERS = 2016;
        const int SET_PARAMETER_PERSISTANCE = 2017;
        const int DEVICE_BEEP_CONTROL = 2018;
        const int REBOOT_SCANNER = 2019;
        const int DISCONNECT_BT_SCANNER = 2023;
        const int DEVICE_CAPTURE_IMAGE = 3000;
        const int ABORT_IMAGE_XFER = 3001;
        const int DEVICE_CAPTURE_BARCODE = 3500;
        const int DEVICE_CAPTURE_VIDEO = 4000;
        public const int RSM_ATTR_GETALL = 5000;
        public const int RSM_ATTR_GET = 5001;
        public const int RSM_ATTR_GETNEXT = 5002;
        public const int RSM_ATTR_SET = 5004;
        public const int RSM_ATTR_STORE = 5005;
        const int GET_DEVICE_TOPOLOGY = 5006;
        const int START_NEW_FIRMWARE = 5014;
        const int UPDATE_ATTRIB_META_FILE = 5015;
        const int UPDATE_FIRMWARE = 5016;
        const int UPDATE_FIRMWARE_FROM_PLUGIN = 5017;
        const int UPDATE_DECODE_TONE = 5050;
        const int ERASE_DECODE_TONE = 5051;
        const int UPDATE_ELECTRIC_FENCE_CUSTOM_TONE = 5052;
        const int ERASE_ELECTRIC_FENCE_CUSTOM_TONE = 5053;
        const int SET_ACTION = 6000;

        const int PAGER_MOTOR_ACTION = 6033;

        const int KEYBOARD_EMULATOR_ENABLE = 6300; //6300
        const int KEYBOARD_EMULATOR_SET_LOCALE = 6301; //6301
        const int KEYBOARD_EMULATOR_GET_CONFIG = 6302; //6302

        const int CONFIGURE_DADF = 6400;
        const int RESET_DADF = 6401;

        // Serial //
        const int DEVICE_SET_SERIAL_PORT_SETTINGS = 6101;
        // Serial - end //

        // USBHIDKB //
        const int DEVICE_SWITCH_HOST_MODE = 6200;
        const int SWITCH_CDC_DEVICES = 6201;
        // USBHIDKB - end //

        //Scale Commands //
        const int SCALE_READ_WEIGHT = 0x1b58; //7000
        const int SCALE_ZERO_SCALE = 0X1B5A; //7002
        const int SCALE_SYSTEM_RESET = 0X1B67; //7015
        //Scale Commands //

        //Wave file Buffer Size (Default File Size is 10KB)//
        const int WAV_FILE_MAX_SIZE = 10240;

        //****** END OF CORESCANNER PROTOCOL *********//
        #endregion

        /* Maximum number of scanners to be connected*/
        const int MAX_NUM_DEVICES = 1;
        // available values for 'status' //
        const int STATUS_SUCCESS = 0;
        const int STATUS_FALSE = 1;

        int status; // Extended API return code

        bool IsScannerDriverInstalled;

        string ScanBarcodeNumber = "";
        string ScanBarcodeSyblogy = "";

        public Form1()
        {
            InitializeComponent();

            m_bSuccessOpen = false;
            m_nTotalScanners = 0;
            m_arScanners = new Scanner[MAX_NUM_DEVICES];
            for (int i = 0; i < MAX_NUM_DEVICES; i++)
            {
                Scanner scanr = new Scanner();
                m_arScanners.SetValue(scanr, i);
            }
            m_xml = new XmlReader();

            ScannerDriverInstalled();
        }

        void ScannerDriverInstalled()
        {
            try
            {
                m_pCoreScanner = new CoreScanner.CCoreScannerClass();

                // Subscribe for barcode events in cCoreScannerClass
                m_pCoreScanner.BarcodeEvent += new _ICoreScannerEvents_BarcodeEventEventHandler(OnBarcodeEvent);

                IsScannerDriverInstalled = true;
            }
            catch (Exception)
            {
                Thread.Sleep(1000);
                IsScannerDriverInstalled = false;
                //m_pCoreScanner = new CoreScanner.CCoreScannerClass();
            }

            ////Call Open API
            //scannerTypes = new short[1];//Scanner Types you are interested in
            //scannerTypes[0] = 1; // 1 for all scanner types
            ////short numberOfScannerTypes = 1; // Size of the scannerTypes array
        }

        void OnBarcodeEvent(short eventType, ref string pscanData)
        {
            try
            {
                //if (!CartonCompleteChecking())
                //{
                //    string tmpScanData = pscanData;
                //    ShowBarcodeLabel(tmpScanData);
                //    AddCartonSerialNumbersList();
                //}
                //else
                //{
                //    XtraMessageBox.Show("This carton is complete. Select empty carton to continue or add new carton.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //}
                string tmpScanData = pscanData;
                ShowBarcodeLabel(tmpScanData);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "Exception OnBarcodeEvent()");

                //int line = Functions.GetLineNumber(ex);
                ////Functions.ErrorLogs("Exception", line, ex.Message, "OnBarcodeEvent()");
            }
        }

        /// <summary>
        /// Populate Barcode data controls
        /// </summary>
        /// <param name="strXml">Barcode data XML</param>
        private void ShowBarcodeLabel(string strXml)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Initial XML" + strXml);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strXml);

                string strData = String.Empty;
                string barcode = xmlDoc.DocumentElement.GetElementsByTagName("datalabel").Item(0).InnerText;
                string symbology = xmlDoc.DocumentElement.GetElementsByTagName("datatype").Item(0).InnerText;
                string[] numbers = barcode.Split(' ');

                foreach (string number in numbers)
                {
                    if (String.IsNullOrEmpty(number))
                    {
                        break;
                    }

                    strData += ((char)Convert.ToInt32(number, 16)).ToString();
                }

                //if (txtTestBarcode.InvokeRequired)
                //{
                //    txtTestBarcode.Invoke(new MethodInvoker(delegate
                //    {
                //        txtTestBarcode.EditValue = "";
                //        txtTestBarcode.Text = strData;
                //    }));
                //}
                ScanBarcodeNumber = strData;

                //if (txtTestSyblogy.InvokeRequired)
                //{
                //    txtTestSyblogy.Invoke(new MethodInvoker(delegate
                //    {
                //        txtTestSyblogy.Text = GetSymbology((int)Convert.ToInt32(symbology));
                //    }));
                //}
                ScanBarcodeSyblogy = GetSymbology((int)Convert.ToInt32(symbology));
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "Exception ShowBarcodeLabel()");

                //int line = Functions.GetLineNumber(ex);
                ////Functions.ErrorLogs("Exception", line, ex.Message, "ShowBarcodeLabel()");
            }
        }

        /// <summary>
        /// Barcode symbology
        /// </summary>
        /// <param name="Code">Symbology code</param>
        /// <returns>Symbology name</returns>
        private string GetSymbology(int Code)
        {
            switch (Code)
            {
                case ST_NOT_APP:
                    return "NOT APPLICABLE";
                case ST_CODE_39:
                    return "CODE 39";
                case ST_CODABAR:
                    return "CODABAR";
                case ST_CODE_128:
                    return "CODE 128";
                case ST_D2OF5:
                    return "DISCRETE 2 OF 5";
                case ST_IATA:
                    return "IATA";
                case ST_I2OF5:
                    return "INTERLEAVED 2 OF 5";
                case ST_CODE93:
                    return "CODE 93";
                case ST_UPCA:
                    return "UPC-A";
                case ST_UPCE0:
                    return "UPC-E0";
                case ST_EAN8:
                    return "EAN-8";
                case ST_EAN13:
                    return "EAN-13";
                case ST_CODE11:
                    return "CODE 11";
                case ST_CODE49:
                    return "CODE 49";
                case ST_MSI:
                    return "MSI";
                case ST_EAN128:
                    return "EAN-128";
                case ST_UPCE1:
                    return "UPC-E1";
                case ST_PDF417:
                    return "PDF-417";
                case ST_CODE16K:
                    return "CODE 16K";
                case ST_C39FULL:
                    return "CODE 39 FULL ASCII";
                case ST_UPCD:
                    return "UPC-D";
                case ST_TRIOPTIC:
                    return "CODE 39 TRIOPTIC";
                case ST_BOOKLAND:
                    return "BOOKLAND";
                case ST_UPCA_W_CODE128:
                    return "UPC-A w/Code 128 Supplemental";
                case ST_JAN13_W_CODE128:
                    return "EAN/JAN-13 w/Code 128 Supplemental";
                case ST_NW7:
                    return "NW-7";
                case ST_ISBT128:
                    return "ISBT-128";
                case ST_MICRO_PDF:
                    return "MICRO PDF";
                case ST_DATAMATRIX:
                    return "DATAMATRIX";
                case ST_QR_CODE:
                    return "QR CODE";
                case ST_MICRO_PDF_CCA:
                    return "MICRO PDF CCA";
                case ST_POSTNET_US:
                    return "POSTNET US";
                case ST_PLANET_CODE:
                    return "PLANET CODE";
                case ST_CODE_32:
                    return "CODE 32";
                case ST_ISBT128_CON:
                    return "ISBT-128 CON";
                case ST_JAPAN_POSTAL:
                    return "JAPAN POSTAL";
                case ST_AUS_POSTAL:
                    return "AUS POSTAL";
                case ST_DUTCH_POSTAL:
                    return "DUTCH POSTAL";
                case ST_MAXICODE:
                    return "MAXICODE";
                case ST_CANADIN_POSTAL:
                    return "CANADIAN POSTAL";
                case ST_UK_POSTAL:
                    return "UK POSTAL";
                case ST_MACRO_PDF:
                    return "MACRO PDF";
                case ST_MACRO_QR_CODE:
                    return "MACRO QR CODE";
                case ST_MICRO_QR_CODE:
                    return "MICRO QR CODE";
                case ST_AZTEC:
                    return "AZTEC";
                case ST_AZTEC_RUNE:
                    return "AZTEC RUNE";
                case ST_DISTANCE:
                    return "DISTANCE";
                case ST_GS1_DATABAR:
                    return "GS1 DATABAR";
                case ST_GS1_DATABAR_LIMITED:
                    return "GS1 DATABAR LIMITED";
                case ST_GS1_DATABAR_EXPANDED:
                    return "GS1 DATABAR EXPANDED";
                case ST_PARAMETER:
                    return "PARAMETER";
                case ST_USPS_4CB:
                    return "USPS 4CB";
                case ST_UPU_FICS_POSTAL:
                    return "UPU FICS POSTAL";
                case ST_ISSN:
                    return "ISSN";
                case ST_SCANLET:
                    return "SCANLET";
                case ST_CUECODE:
                    return "CUECODE";
                case ST_MATRIX2OF5:
                    return "MATRIX 2 OF 5";
                case ST_UPCA_2:
                    return "UPC-A + 2 SUPPLEMENTAL";
                case ST_UPCE0_2:
                    return "UPC-E0 + 2 SUPPLEMENTAL";
                case ST_EAN8_2:
                    return "EAN-8 + 2 SUPPLEMENTAL";
                case ST_EAN13_2:
                    return "EAN-13 + 2 SUPPLEMENTAL";
                case ST_UPCE1_2:
                    return "UPC-E1 + 2 SUPPLEMENTAL";
                case ST_CCA_EAN128:
                    return "CCA EAN-128";
                case ST_CCA_EAN13:
                    return "CCA EAN-13";
                case ST_CCA_EAN8:
                    return "CCA EAN-8";
                case ST_CCA_RSS_EXPANDED:
                    return "GS1 DATABAR EXPANDED COMPOSITE (CCA)";
                case ST_CCA_RSS_LIMITED:
                    return "GS1 DATABAR LIMITED COMPOSITE (CCA)";
                case ST_CCA_RSS14:
                    return "GS1 DATABAR COMPOSITE (CCA)";
                case ST_CCA_UPCA:
                    return "CCA UPC-A";
                case ST_CCA_UPCE:
                    return "CCA UPC-E";
                case ST_CCC_EAN128:
                    return "CCA EAN-128";
                case ST_TLC39:
                    return "TLC-39";
                case ST_CCB_EAN128:
                    return "CCB EAN-128";
                case ST_CCB_EAN13:
                    return "CCB EAN-13";
                case ST_CCB_EAN8:
                    return "CCB EAN-8";
                case ST_CCB_RSS_EXPANDED:
                    return "GS1 DATABAR EXPANDED COMPOSITE (CCB)";
                case ST_CCB_RSS_LIMITED:
                    return "GS1 DATABAR LIMITED COMPOSITE (CCB)";
                case ST_CCB_RSS14:
                    return "GS1 DATABAR COMPOSITE (CCB)";
                case ST_CCB_UPCA:
                    return "CCB UPC-A";
                case ST_CCB_UPCE:
                    return "CCB UPC-E";
                case ST_SIGNATURE_CAPTURE:
                    return "SIGNATURE CAPTUREE";
                case ST_MOA:
                    return "MOA";
                case ST_PDF417_PARAMETER:
                    return "PDF417 PARAMETER";
                case ST_CHINESE2OF5:
                    return "CHINESE 2 OF 5";
                case ST_KOREAN_3_OF_5:
                    return "KOREAN 3 OF 5";
                case ST_DATAMATRIX_PARAM:
                    return "DATAMATRIX PARAM";
                case ST_CODE_Z:
                    return "CODE Z";
                case ST_UPCA_5:
                    return "UPC-A + 5 SUPPLEMENTAL";
                case ST_UPCE0_5:
                    return "UPC-E0 + 5 SUPPLEMENTAL";
                case ST_EAN8_5:
                    return "EAN-8 + 5 SUPPLEMENTAL";
                case ST_EAN13_5:
                    return "EAN-13 + 5 SUPPLEMENTAL";
                case ST_UPCE1_5:
                    return "UPC-E1 + 5 SUPPLEMENTAL";
                case ST_MACRO_MICRO_PDF:
                    return "MACRO MICRO PDF";
                case ST_OCRB:
                    return "OCRB";
                case ST_OCRA:
                    return "OCRA";
                case ST_PARSED_DRIVER_LICENSE:
                    return "PARSED DRIVER LICENSE";
                case ST_PARSED_UID:
                    return "PARSED UID";
                case ST_PARSED_NDC:
                    return "PARSED NDC";
                case ST_DATABAR_COUPON:
                    return "DATABAR COUPON";
                case ST_PARSED_XML:
                    return "PARSED XML";
                case ST_HAN_XIN_CODE:
                    return "HAN XIN CODE";
                case ST_CALIBRATION:
                    return "CALIBRATION";
                case ST_GS1_DATAMATRIX:
                    return "GS1 DATA MATRIX";
                case ST_GS1_QR:
                    return "GS1 QR";
                case BT_MAINMARK:
                    return "MAIL MARK";
                case BT_DOTCODE:
                    return "DOT CODE";
                case BT_GRID_MATRIX:
                    return "GRID MATRIX";
                case ST_EPC_RAW:
                    return "EPC RAW";
                case BT_UDI_CODE:
                    return "UDI CODE";
                default:
                    return "";
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (IsScannerDriverInstalled)
            {
                Connect();
                if (status == STATUS_SUCCESS)
                    GetScanners();
                else
                    Disconnect();
            }
            else
            {
                //lblScannerDesc.Text = @"The SDK and driver of the device are not installed on the computer! Please install the 'Scanner SDK (xxbit) v2.0x. .exe' software.";
            }
        }

        /// <summary>
        /// Calls Open command
        /// </summary>
        private void Connect()
        {
            if (m_bSuccessOpen)
            {
                return;
            }
            int appHandle = 0;
            //GetSelectedScannerTypes();
            short[] scannerTypes = new short[1];//Scanner Types you are interested in
            scannerTypes[0] = 1; // 1 for all scanner types
            short numberOfScannerTypes = 1; // Size of the scannerTypes array

            int status = STATUS_FALSE;

            try
            {
                m_pCoreScanner.Open(appHandle, scannerTypes, numberOfScannerTypes, out status);
                //DisplayResult(status, "OPEN");
                if (STATUS_SUCCESS == status)
                {
                    m_bSuccessOpen = true;
                }
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "Exception Connect()");
                //int line = Functions.GetLineNumber(ex);
                ////Functions.ErrorLogs("Exception", line, ex.Message, "Connect()");
                MessageBox.Show("Error OPEN - " + ex.Message, "APP_TITLE", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                if (STATUS_SUCCESS == status)
                {
                    //SetControls();
                }
            }
        }

        void GetScanners()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                ////Instantiate CoreScanner Class
                //cCoreScannerClass = new CoreScanner.CCoreScannerClass();

                ////Call Open API
                //short[] scannerTypes = new short[1];//Scanner Types you are interested in
                //scannerTypes[0] = 1; // 1 for all scanner types
                //short numberOfScannerTypes = 1; // Size of the scannerTypes array
                //int status; // Extended API return code

                //cCoreScannerClass.Open(0, scannerTypes, numberOfScannerTypes, out status);

                // Lets list down all the scanners connected to the host
                short numberOfScanners; // Number of scanners expect to be used
                int[] connectedScannerIDList = new int[255];// List of scanner IDs to be returned

                string outXML; //Scanner details output

                //if (status == 0)
                //{
                m_pCoreScanner.GetScanners(out numberOfScanners, connectedScannerIDList, out outXML, out status);

                Console.WriteLine(outXML);

                m_nTotalScanners = numberOfScanners;
                if (m_nTotalScanners > 0)
                {
                    int nScannerCount = m_nTotalScanners;
                    m_xml.ReadXmlString_GetScanners(outXML, m_arScanners, numberOfScanners, out nScannerCount);
                    for (int index = 0; index < m_arScanners.Length; index++)
                    {
                        string modelNo = "";
                        modelNo = "'" + m_arScanners[index].MODELNO + "'";
                        string serialNo = "";
                        serialNo = "'" + m_arScanners[index].SERIALNO + "'";
                        //lblScannerDesc.Text = "Scanner device with model number " + modelNo + " is connected. (Serial Number: " + serialNo + ")"; //if (string.Compare(claimlist[i], m_arScanners[index].SERIALNO) == 0)

                    }

                    //bsiBarcodeScanner.Caption = "Barcode scanner connected: YES";
                    //lblConnScanner.Text = "Device connected";
                    //System.ComponentModel.ComponentResourceManager resources =
                    //    new System.ComponentModel.ComponentResourceManager(typeof(BridgeSystemsLogisticApp.Properties.Resources));
                    //Image resImage = ((System.Drawing.Image)(resources.GetObject("DefaultBarcode_32x32")));
                    //pictureEdit1.Image = resImage;
                }
                //}
                //else
                //{
                //    Console.WriteLine("CoreScanner API: Open Failed");
                //}
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                //logger.Error(ex, "Exception GetScanners()");
                //int line = Functions.GetLineNumber(ex);
                ////Functions.ErrorLogs("Exception", line, ex.Message, "GetScanners()");
                ////Console.WriteLine("Something wrong please check... " + ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// Calls Close command
        /// </summary>
        private void Disconnect()
        {
            if (m_bSuccessOpen)
            {
                int appHandle = 0;
                int status = STATUS_FALSE;
                try
                {
                    m_pCoreScanner.Close(appHandle, out status);
                    //DisplayResult(status, "CLOSE");
                    if (STATUS_SUCCESS == status)
                    {
                        m_bSuccessOpen = false;
                        //lstvScanners.Items.Clear();
                        //combSlcrScnr.Items.Clear();
                        m_nTotalScanners = 0;
                        //InitScannersCount();
                        //UpdateScannerCountLabels();
                        //SetControls();
                    }
                }
                catch (Exception ex)
                {
                    //logger.Error(ex, "Exception Disconnect()");

                    //int line = Functions.GetLineNumber(ex);
                    ////Functions.ErrorLogs("Exception", line, ex.Message, "Disconnect()");
                    MessageBox.Show("CLOSE Error - " + ex.Message, "APP_TITLE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
