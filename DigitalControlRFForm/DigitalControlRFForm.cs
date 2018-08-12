﻿using COMMPortLib;
using DigitalControlRFLib;
using MessageBoxPlusLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalControlRFForm
{
	public partial class DigitalControlRFMenuForm : Form
	{
		#region 变量定义

		private DigitalControlRF usedDigitalControlRF = null;


		#endregion

		#region 窗体加载初始化
		public DigitalControlRFMenuForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 消息处理函数
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			if (this.usedDigitalControlRF != null)
			{
				this.usedDigitalControlRF.DevicesChangedEvent(ref m, this.comboBox_PortName, this.richTextBox_Msg);
			}
			base.WndProc(ref m);
		}
		/// <summary>
		/// 船体加载事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DigitalControlRFMenuForm_Load(object sender, EventArgs e)
		{
			this.DigitalControlRFMenuForm_Init();
		}

		/// <summary>
		/// 船体关闭事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DigitalControlRFMenuForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.MdiFormClosing)
			{
				return;
			}
			else if (e.CloseReason == CloseReason.UserClosing)
			{
				if (DialogResult.OK == MessageBoxPlus.Show(this,"你确定要关闭应用程序吗？", "关闭提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
				{
					//---为保证Application.Exit();时不再弹出提示，所以将FormClosing事件取消
					this.FormClosing -= new FormClosingEventHandler(this.DigitalControlRFMenuForm_FormClosing);
					//---确认关闭事件
					e.Cancel = false;
					//退出当前应用---如果是MDI窗体,会导致整个应用退出
					//Application.Exit();
					//---退出当前窗体
					this.Dispose();
				}
				else
				{
					//---取消关闭事件
					e.Cancel = true;
				}
			}
			
		}

		/// <summary>
		/// 窗体位置改变事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DigitalControlRFMenuForm_LocationChanged(object sender, EventArgs e)
		{
			this.StartPosition = FormStartPosition.CenterScreen;
		}

		#endregion

		#region 初始化函数

		/// <summary>
		/// 
		/// </summary>
		private void DigitalControlRFMenuForm_Init()
		{
			//---限定船体的最小值
			this.MinimumSize = this.Size;
			//---限定窗体的最大值
			//this.MaximumSize = this.Size;
			if (this.usedDigitalControlRF==null)
			{
				this.usedDigitalControlRF=new DigitalControlRF(this,new SerialCOMMPort(this),this.comboBox_PortName,this.richTextBox_Msg);
			}
			//---事件注册
			this.RegistrationEvent();
		}



		#endregion

		#region 控件事件
		/// <summary>
		/// 注册事件
		/// </summary>
		private void RegistrationEvent()
		{
			this.button_OpenDevice.Click += new EventHandler(this.button_Click);
			this.button_CloseDevice.Click += new EventHandler(this.button_Click);

			//---信号源功率
			this.button_Read433MSRFPower.Click += new EventHandler(this.button_Click);
			this.button_Write433MSRFPower.Click += new EventHandler(this.button_Click);

			this.button_Read315MRFPower.Click += new EventHandler(this.button_Click);
			this.button_Write315MRFPower.Click += new EventHandler(this.button_Click);

			this.button_Read207MRFPower.Click += new EventHandler(this.button_Click);
			this.button_Write207MRFPower.Click += new EventHandler(this.button_Click);

			this.button_Read433MBRFPower.Click += new EventHandler(this.button_Click);
			this.button_Write433MBRFPower.Click += new EventHandler(this.button_Click);


			//---信号源衰减
			this.button_ReadRF433MS.Click += new EventHandler(this.button_Click);
			this.button_WriteRF433MS.Click += new EventHandler(this.button_Click);

			this.button_ReadRF315M.Click += new EventHandler(this.button_Click);
			this.button_WriteRF315M.Click += new EventHandler(this.button_Click);

			this.button_ReadRF207M.Click += new EventHandler(this.button_Click);
			this.button_WriteRF207M.Click += new EventHandler(this.button_Click);

			this.button_ReadRF433MB.Click += new EventHandler(this.button_Click);
			this.button_WriteRF433MB.Click += new EventHandler(this.button_Click);

			//---输出的型号衰减
			this.button_ReadRFA.Click += new EventHandler(this.button_Click);
			this.button_WriteRFA.Click += new EventHandler(this.button_Click);

			this.button_ReadRFB.Click += new EventHandler(this.button_Click);
			this.button_WriteRFB.Click += new EventHandler(this.button_Click);

			this.button_ReadRFC.Click += new EventHandler(this.button_Click);
			this.button_WriteRFC.Click += new EventHandler(this.button_Click);

			this.button_ReadRFD.Click += new EventHandler(this.button_Click);
			this.button_WriteRFD.Click += new EventHandler(this.button_Click);

			this.button_ReadRFE.Click += new EventHandler(this.button_Click);
			this.button_WriteRFE.Click += new EventHandler(this.button_Click);

			this.button_ReadRFF.Click += new EventHandler(this.button_Click);
			this.button_WriteRFF.Click += new EventHandler(this.button_Click);

			this.button_ReadRFG.Click += new EventHandler(this.button_Click);
			this.button_WriteRFG.Click += new EventHandler(this.button_Click);

			this.button_ReadRFH.Click += new EventHandler(this.button_Click);
			this.button_WriteRFH.Click += new EventHandler(this.button_Click);

			//加载contextMenuTrip的子项---消息显示的清楚和 
			ToolStripItem tsItem;
			tsItem = this.AddContextMenu("清除", this.contextMenuStrip_richTextBox_Msg.Items, new EventHandler(this.RichTextBoxMsgClearButton_Click));
		}

		/// <summary>
		/// 卸载事件
		/// </summary>
		private void UnRegistrationEvent()
		{

		}
		/// <summary>  button_Read433MSRFPower
		/// 添加子菜单  button_Write433MSRFPowe
		/// </summary>  
		/// <param name="text">要显示的文字，如果为 - 则显示为分割线</param>  
		/// <param name="cms">要添加到的子菜单集合</param>  
		/// <param name="callback">点击时触发的事件</param>  
		/// <returns>生成的子菜单，如果为分隔条则返回null</returns>  
		private ToolStripMenuItem AddContextMenu(string text, ToolStripItemCollection cms, EventHandler callback)
		{
			if (text == "-")
			{
				ToolStripSeparator tsp = new ToolStripSeparator();
				cms.Add(tsp);
				return null;
			}
			else if (!string.IsNullOrEmpty(text))
			{
				ToolStripMenuItem tsmi = new ToolStripMenuItem(text);
				if (callback != null)
				{
					tsmi.Click += callback;
				}
				cms.Add(tsmi);
				return tsmi;
			}
			return null;
		}


		/// <summary>
		/// 删除操作
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RichTextBoxMsgClearButton_Click(object sender, EventArgs e)
		{
			this.richTextBox_Msg.Clear();
		}
		/// <summary>
		/// 按钮点击事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button_Click(object sender, EventArgs e)
		{
			Button btn = (Button)sender;
			btn.Enabled = false;
			switch (btn.Name)
			{
				case "button_OpenDevice":
					if (this.usedDigitalControlRF!=null)
					{
						this.usedDigitalControlRF.OpenDevice(this.comboBox_PortName.Text, this.richTextBox_Msg);
					}
					break;
				case "button_CloseDevice":
					if (this.usedDigitalControlRF != null)
					{
						this.usedDigitalControlRF.CloseDevice(this.comboBox_PortName.Text, this.richTextBox_Msg);
					}
					break;
				case "button_Read433MSRFPower":
					if (this.usedDigitalControlRF != null)
					{
						this.usedDigitalControlRF.ReadRFPower433MS(this.numericUpDown_433MSRFPower, this.richTextBox_Msg);
					}
					break;
				case "button_Write433MSRFPower":
					if (this.usedDigitalControlRF != null)
					{
						this.usedDigitalControlRF.WriteRFPower433MS(this.numericUpDown_433MSRFPower, this.richTextBox_Msg);
					}
					break;
				case "button_Read315MRFPower":
					if (this.usedDigitalControlRF != null)
					{
						this.usedDigitalControlRF.ReadRFPower315M(this.numericUpDown_315MRFPower, this.richTextBox_Msg);
					}
					break;
				case "button_Write315MRFPower":
					if (this.usedDigitalControlRF != null)
					{
						this.usedDigitalControlRF.WriteRFPower315M(this.numericUpDown_315MRFPower, this.richTextBox_Msg);
					}
					break;
				case "button_Read207MRFPower":
					if (this.usedDigitalControlRF != null)
					{
						this.usedDigitalControlRF.ReadRFPower207M(this.numericUpDown_207MRFPower, this.richTextBox_Msg);
					}
					break;
				case "button_Write207MRFPower":
					if (this.usedDigitalControlRF != null)
					{
						this.usedDigitalControlRF.WriteRFPower207M(this.numericUpDown_207MRFPower, this.richTextBox_Msg);
					}
					break;
				case "button_Read433MBRFPower":
					if (this.usedDigitalControlRF != null)
					{
						this.usedDigitalControlRF.ReadRFPower433MB(this.numericUpDown_433MBRFPower, this.richTextBox_Msg);
					}
					break;
				case "button_Write433MBRFPower":
					if (this.usedDigitalControlRF != null)
					{
						this.usedDigitalControlRF.WriteRFPower433MB(this.numericUpDown_433MBRFPower, this.richTextBox_Msg);
					}
					break;
				case "button_ReadRF433MS":
					if (this.usedDigitalControlRF != null)
					{
						this.usedDigitalControlRF.ReadRF433MS(this.numericUpDown_RF433MS, this.richTextBox_Msg);
					}
					break;
				case "button_WriteRF433MS":
					if (this.usedDigitalControlRF != null)
					{
						this.usedDigitalControlRF.WriteRF433MS(this.numericUpDown_RF433MS, this.richTextBox_Msg);
					}
					break;
				default:
					break;
			}
			btn.Enabled = true;
		}

		#endregion
	}
}
