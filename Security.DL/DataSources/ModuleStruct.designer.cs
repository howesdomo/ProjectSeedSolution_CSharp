﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Security.DL.DataSources
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	public partial class ModuleStructDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region 可扩展性方法定义
    partial void OnCreated();
    partial void InsertSysModuleMTR(SysModuleMTR instance);
    partial void UpdateSysModuleMTR(SysModuleMTR instance);
    partial void DeleteSysModuleMTR(SysModuleMTR instance);
    partial void InsertSysPromissionMTR(SysPromissionMTR instance);
    partial void UpdateSysPromissionMTR(SysPromissionMTR instance);
    partial void DeleteSysPromissionMTR(SysPromissionMTR instance);
    #endregion
		
		public ModuleStructDataContext() : 
				base(global::Security.DL.Properties.Settings.Default.Security_WPFConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public ModuleStructDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ModuleStructDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ModuleStructDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ModuleStructDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<SysModuleMTR> SysModuleMTR
		{
			get
			{
				return this.GetTable<SysModuleMTR>();
			}
		}
		
		public System.Data.Linq.Table<SysPromissionMTR> SysPromissionMTR
		{
			get
			{
				return this.GetTable<SysPromissionMTR>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.SysModuleMTR")]
	public partial class SysModuleMTR : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _ID;
		
		private string _ModuleName;
		
		private EntitySet<SysPromissionMTR> _SysPromissionMTR;
		
    #region 可扩展性方法定义
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(System.Guid value);
    partial void OnIDChanged();
    partial void OnModuleNameChanging(string value);
    partial void OnModuleNameChanged();
    #endregion
		
		public SysModuleMTR()
		{
			this._SysPromissionMTR = new EntitySet<SysPromissionMTR>(new Action<SysPromissionMTR>(this.attach_SysPromissionMTR), new Action<SysPromissionMTR>(this.detach_SysPromissionMTR));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ModuleName", DbType="NVarChar(50)")]
		public string ModuleName
		{
			get
			{
				return this._ModuleName;
			}
			set
			{
				if ((this._ModuleName != value))
				{
					this.OnModuleNameChanging(value);
					this.SendPropertyChanging();
					this._ModuleName = value;
					this.SendPropertyChanged("ModuleName");
					this.OnModuleNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="SysModuleMTR_SysPromissionMTR", Storage="_SysPromissionMTR", ThisKey="ID", OtherKey="ModuleID")]
		public EntitySet<SysPromissionMTR> SysPromissionMTR
		{
			get
			{
				return this._SysPromissionMTR;
			}
			set
			{
				this._SysPromissionMTR.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_SysPromissionMTR(SysPromissionMTR entity)
		{
			this.SendPropertyChanging();
			entity.SysModuleMTR = this;
		}
		
		private void detach_SysPromissionMTR(SysPromissionMTR entity)
		{
			this.SendPropertyChanging();
			entity.SysModuleMTR = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.SysPromissionMTR")]
	public partial class SysPromissionMTR : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _ID;
		
		private System.Nullable<System.Guid> _ModuleID;
		
		private string _PermissionName;
		
		private string _SysCode;
		
		private System.Nullable<int> _Seq;
		
		private EntityRef<SysModuleMTR> _SysModuleMTR;
		
    #region 可扩展性方法定义
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(System.Guid value);
    partial void OnIDChanged();
    partial void OnModuleIDChanging(System.Nullable<System.Guid> value);
    partial void OnModuleIDChanged();
    partial void OnPermissionNameChanging(string value);
    partial void OnPermissionNameChanged();
    partial void OnSysCodeChanging(string value);
    partial void OnSysCodeChanged();
    partial void OnSeqChanging(System.Nullable<int> value);
    partial void OnSeqChanged();
    #endregion
		
		public SysPromissionMTR()
		{
			this._SysModuleMTR = default(EntityRef<SysModuleMTR>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ModuleID", DbType="UniqueIdentifier")]
		public System.Nullable<System.Guid> ModuleID
		{
			get
			{
				return this._ModuleID;
			}
			set
			{
				if ((this._ModuleID != value))
				{
					if (this._SysModuleMTR.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnModuleIDChanging(value);
					this.SendPropertyChanging();
					this._ModuleID = value;
					this.SendPropertyChanged("ModuleID");
					this.OnModuleIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PermissionName", DbType="NVarChar(50)")]
		public string PermissionName
		{
			get
			{
				return this._PermissionName;
			}
			set
			{
				if ((this._PermissionName != value))
				{
					this.OnPermissionNameChanging(value);
					this.SendPropertyChanging();
					this._PermissionName = value;
					this.SendPropertyChanged("PermissionName");
					this.OnPermissionNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SysCode", DbType="NVarChar(50)")]
		public string SysCode
		{
			get
			{
				return this._SysCode;
			}
			set
			{
				if ((this._SysCode != value))
				{
					this.OnSysCodeChanging(value);
					this.SendPropertyChanging();
					this._SysCode = value;
					this.SendPropertyChanged("SysCode");
					this.OnSysCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Seq", DbType="int")]
		public System.Nullable<int> Seq
		{
			get
			{
				return this._Seq;
			}
			set
			{
				if ((this._Seq != value))
				{
					this.OnSeqChanging(value);
					this.SendPropertyChanging();
					this._Seq = value;
					this.SendPropertyChanged("Seq");
					this.OnSeqChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="SysModuleMTR_SysPromissionMTR", Storage="_SysModuleMTR", ThisKey="ModuleID", OtherKey="ID", IsForeignKey=true, DeleteRule="CASCADE")]
		internal SysModuleMTR SysModuleMTR
		{
			get
			{
				return this._SysModuleMTR.Entity;
			}
			set
			{
				SysModuleMTR previousValue = this._SysModuleMTR.Entity;
				if (((previousValue != value) 
							|| (this._SysModuleMTR.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._SysModuleMTR.Entity = null;
						previousValue.SysPromissionMTR.Remove(this);
					}
					this._SysModuleMTR.Entity = value;
					if ((value != null))
					{
						value.SysPromissionMTR.Add(this);
						this._ModuleID = value.ID;
					}
					else
					{
						this._ModuleID = default(Nullable<System.Guid>);
					}
					this.SendPropertyChanged("SysModuleMTR");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
