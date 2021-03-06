﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;

namespace COMMPortLib
{
	/// <summary>
	/// 消息
	/// </summary>
	public struct CWndProMsgConst
	{
		public const int WM_DEVICECHANGE = 0x219; // 系统硬件改变发出的系统消息
		public const int DBT_DEVICEARRIVAL = 0x8000;// 设备检测结束，并且可以使用
		public const int DBT_CONFIGCHANGECANCELED = 0x0019;
		public const int DBT_CONFIGCHANGED = 0x0018;
		public const int DBT_CUSTOMEVENT = 0x8006;
		public const int DBT_DEVICEQUERYREMOVE = 0x8001;
		public const int DBT_DEVICEQUERYREMOVEFAILED = 0x8002;
		public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;// 设备卸载或者拔出
		public const int DBT_DEVICEREMOVEPENDING = 0x8003;
		public const int DBT_DEVICETYPEHANGED = 0x0007;
		public const int DBT_QUERYCHANGSPECIFIC = 0x8005;
		public const int DBT_DEVNODES_CECONFIG = 0x0017;
		public const int DBT_USERDEFINED = 0xFFFF;
	}

	/// <summary>
	/// 使用的CRC校验方式
	/// </summary>
	public enum USE_CRC : byte
	{
		CRC_NONE = 0,
		CRC_CHECKSUM = 1,
		CRC_CRC8 = 2,
		CRC_CRC16 = 3,
		CRC_CRC32 = 4,
	};

	/// <summary>
	/// 通信设备
	/// </summary>
	public struct COMMDevice
	{
		/// <summary>
		/// 设备名称
		/// </summary>
		public string name;

		/// <summary>
		/// 设备序号
		/// </summary>
		public int index;

		/// <summary>
		/// 设备状态;false---不可用,true---可用
		/// </summary>
		public bool state;

		#region 初始化

		public void Init()
		{
			this.name = string.Empty;
			this.index = 0;
			this.state = false;
		}

		#endregion 初始化
	}

	public class COMMPortBytes
	{
		/// <summary>
		/// 数据
		/// </summary>
		public List<byte> dateBytes = null;

		/// <summary>
		/// 使用的CRC的等级
		/// </summary>
		public byte crc = (byte)USE_CRC.CRC_NONE;

		/// <summary>
		/// crc的值
		/// </summary>
		public UInt32 crcVal = 0;

		#region 初始化

		public COMMPortBytes()
		{
			if (this.dateBytes == null)
			{
				this.dateBytes = new List<byte>();
			}
		}

		#endregion 初始化
	}

	public class COMMPort
	{
		#region 变量定义

		/// <summary>
		///
		/// </summary>
		private string commPortName = null;

		/// <summary>
		///
		/// </summary>
		private int commPortIndex = 0;

		/// <summary>
		/// 工作状态，忙---true；空闲---false
		/// </summary>
		private bool commPortSTATE = false;

        /// <summary>
        /// 是否需要刷新设备
        /// </summary>
        private bool commPortRefresh = false;

        /// <summary>
        /// 当前端口发送数据缓存区的大小
        /// </summary>
        private int commPortSize = 64;

		/// <summary>
		/// 接收CRC等级
		/// </summary>
		private byte commPortReceCRC = (byte)USE_CRC.CRC_CRC8;//0;

		/// <summary>
		/// 发送CRC等级
		/// </summary>
		private byte commPortSendCRC = 0;

		/// <summary>
		/// 接收数据缓存区
		/// </summary>
		private COMMPortBytes commPortReceBytes = null;

		/// <summary>
		/// 发送数据缓存区
		/// </summary>
		private COMMPortBytes commPortSendBytes = null;

		/// <summary>
		/// 耗时时间
		/// </summary>
		private TimeSpan commPortUsedTime;

		/// <summary>
		/// 错误信息
		/// </summary>
		private string commPortErrMsg = string.Empty;

		/// <summary>
		/// 使用的窗体
		/// </summary>
		private Form commPortForm = null;

		/// <summary>
		/// 接收报头
		/// </summary>
		private byte commPortReceID = 0x5A;

		/// <summary>
		/// 发送数据报头
		/// </summary>
		private byte commPortSendID = 0x55;

		/// <summary>
		/// 是否使能数据接收事件
		/// </summary>
		private bool commEnableDataReceivedEvent = false;

		/// <summary>
		/// 当前设备的信息
		/// </summary>
		private COMMDevice[] commPortDevices = null;

		/// <summary>
		/// 设备的ID信息，用于多设备的通信
		/// </summary>
		private int deviceID = 0x00;

		/// <summary>
		/// 设备ID在命令中的位置
		/// </summary>
		private int deviceIDIndex = 0;

        /// <summary>
		/// 使用的串口信息
		/// </summary>
		private SerialPort commSerialPort = null;

        #endregion 变量定义

        #region 属性定义

        /// <summary>
        ///
        /// </summary>
        public virtual string m_COMMPortName
		{
			get
			{
				return this.commPortName;
			}
			set
			{
				this.commPortName = value;
			}
		}

		/// <summary>
		///
		/// </summary>
		public virtual int m_COMMPortIndex
		{
			get
			{
				return this.commPortIndex;
			}
			set
			{
				this.commPortIndex = value;
			}
		}

		/// <summary>
		/// 工作状态，忙---true；空闲---false
		/// </summary>
		public virtual bool m_COMMPortSTATE
		{
			get
			{
				return this.commPortSTATE;
			}
			set
			{
				this.commPortSTATE = value;
			}
		}

        /// <summary>
        /// 
        /// </summary>
        public virtual bool m_COMMPortRefresh
        {
            get
            {
                return this.commPortRefresh;
            }
            set
            {
                this.commPortRefresh = value;
            }
        }

        /// <summary>
        /// 当前端口发送数据缓存区的大小
        /// </summary>
        public virtual int m_COMMPortSize
		{
			get
			{
				return this.commPortSize;
			}
			set
			{
				this.commPortSize = value;
			}
		}

		/// <summary>
		/// 接收CRC等级
		/// </summary>
		public virtual byte m_COMMPortReceCRC
		{
			get
			{
				return this.commPortReceCRC;
			}
			set
			{
				this.m_COMMPortReceCRC = value;
			}
		}

		/// <summary>
		/// 发送CRC等级
		/// </summary>
		public virtual byte m_COMMPortSendCRC
		{
			get
			{
				return this.commPortSendCRC;
			}
			set
			{
				this.commPortSendCRC = value;
			}
		}

		/// <summary>
		/// 接收数据缓存区
		/// </summary>
		public virtual COMMPortBytes m_COMMPortReceBytes
		{
			get
			{
				return this.commPortReceBytes;
			}
			set
			{
				this.commPortReceBytes = value;
			}
		}

		/// <summary>
		/// 发送数据缓存区
		/// </summary>
		public virtual COMMPortBytes m_COMMPortSendBytes
		{
			get
			{
				return this.commPortSendBytes;
			}
			set
			{
				this.commPortSendBytes = value;
			}
		}

		/// <summary>
		/// 耗时时间
		/// </summary>
		public virtual TimeSpan m_COMMPortUsedTime
		{
			get
			{
				return this.commPortUsedTime;
			}
			set
			{
				this.commPortUsedTime = value;
			}
		}

		/// <summary>
		/// 错误信息
		/// </summary>
		public virtual string m_COMMPortErrMsg
		{
			get
			{
				return this.commPortErrMsg;
			}
			set
			{
				this.commPortErrMsg = value;
			}
		}

		/// <summary>
		/// 使用的窗体
		/// </summary>
		public virtual Form m_COMMPortForm
		{
			get
			{
				return this.commPortForm;
			}
			set
			{
				this.commPortForm = value;
			}
		}

		/// <summary>
		/// 接收事件使能
		/// </summary>
		public virtual bool m_COMMEnableDataReceivedEvent
		{
			get
			{
				return this.commEnableDataReceivedEvent;
			}
			set
			{
				this.commEnableDataReceivedEvent = value;
			}
		}

		/// <summary>
		/// 接收报头
		/// </summary>
		public virtual byte m_COMMPortReceID
		{
			get
			{
				return this.commPortReceID;
			}
			set
			{
				this.commPortReceID = value;
			}
		}

		/// <summary>
		/// 发送数据报头
		/// </summary>
		public virtual byte m_COMMPortSendID
		{
			get
			{
				return this.commPortSendID;
			}
			set
			{
				this.commPortSendID = value;
			}
		}

		/// <summary>
		///
		/// </summary>
		public virtual COMMDevice[] m_COMMPortDevices
		{
			get
			{
				return this.commPortDevices;
			}
			set
			{
				this.commPortDevices = value;
			}
		}

		/// <summary>
		///
		/// </summary>
		public virtual int m_DeviceID
		{
			get
			{
				return this.deviceID;
			}
			set
			{
				this.deviceID = value;
				if (this.deviceID > 0)
				{
					this.deviceIDIndex = 1;
				}
				else
				{
					this.deviceIDIndex = 0;
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		public virtual int m_DeviceIDIndex
		{
			get
			{
				return this.deviceIDIndex;
			}
			set
			{
				this.deviceIDIndex = value;
			}
		}


        /// <summary>
        /// 使用的串口
        /// </summary>
        public virtual SerialPort m_COMMSerialPort
        {
            get
            {
                return this.commSerialPort;
            }
            set
            {
                this.commSerialPort = value;
            }
        }

        #endregion 属性定义

        #region 构造函数

        /// <summary>
        ///
        /// </summary>
        public COMMPort()
		{
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="usedForm"></param>
		public COMMPort(Form usedForm)
		{
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="usedForm"></param>
		/// <param name="deviceID"></param>
		public COMMPort(Form usedForm, int deviceID)
		{
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="usedForm"></param>
		public COMMPort(Form usedForm, int bandRate, int size)
		{
		}

		#endregion 构造函数

		#region 函数定义

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public virtual int Init()
		{
			return 1;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="cbb"></param>
		/// <returns></returns>
		public virtual int Init(ComboBox cbb = null)
		{
			return 1;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="cbb"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int Init(ComboBox cbb = null, RichTextBox msg = null)
		{
			return 1;
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public virtual int OpenDevice()
		{
			return 1;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public virtual int OpenDevice(string name)
		{
			return 1;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="name"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int OpenDevice(string name, RichTextBox msg = null)
		{
			return 1;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public virtual int OpenDevice(int index)
		{
			return 1;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="index"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int OpenDevice(int index, RichTextBox msg = null)
		{
			return 1;
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public virtual int CloseDevice()
		{
			return 1;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public virtual int CloseDevice(string name)
		{
			return 1;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="name"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CloseDevice(string name, RichTextBox msg = null)
		{
			return 1;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public virtual int CloseDevice(int index)
		{
			return 1;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="index"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int CloseDevice(int index, RichTextBox msg = null)
		{
			return 1;
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public virtual bool IsAttached()
		{
			return false;
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public virtual bool IsAttached(string name)
		{
			return false;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public virtual bool IsAttached(int index)
		{
			return false;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public virtual int SendCmd(byte cmd)
		{
			return 1;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public virtual int SendCmd(byte[] cmd)
		{
			return 1;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="time"></param>
		/// <returns></returns>
		public virtual int ReadResponse( int time = 200)
		{
			return 1;
		}
		/// <summary>
		///
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="time"></param>
		/// <returns></returns>
		public virtual int ReadResponse(ref byte[] cmd, int time = 200)
		{
			return 1;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="time"></param>
		/// <returns></returns>
		public virtual int ReadResponse(ref List<byte> cmd, int time = 200)
		{
			return 1;
		}
		/// <summary>
		///
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="res"></param>
		/// <param name="time"></param>
		/// <returns></returns>
		public virtual int SendCmdAndReadResponse(byte[] cmd, ref byte[] res, int time = 200)
		{
			return 1;
		}

		/// <summary>
		/// 接收数据是否有效
		/// </summary>
		/// <param name="isOk"></param>
		/// <returns></returns>
		public virtual bool IsReadValidData(int isOk)
		{
			return false;
		}

		#endregion 函数定义

		#region 事件函数


		/// <summary>
		/// 注册事件
		/// </summary>
		/// <param name="msg"></param>
		public virtual void RegisterEventHandler(RichTextBox msg = null)
		{

		}

		/// <summary>
		/// 卸载事件
		/// </summary>
		public virtual void UnRegisterEventHandler()
		{

		}
		/// <summary>
		/// 系统消息通知设备的拔插
		/// </summary>
		/// <param name="m"></param>
		public virtual int DevicesChangedEvent(ref Message m, ComboBox cbb = null, RichTextBox msg = null)
		{
            return 0;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void DataReceivedEvent(object sender, EventArgs e)
		{
		}

		#endregion 事件函数
	}
}