﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace KVP_Obrazci.Domain.KVPOdelo
{

    public partial class Departments : XPLiteObject
    {
        int fId;
        [Key(true)]
        public int Id
        {
            get { return fId; }
            set { SetPropertyValue<int>("Id", ref fId, value); }
        }
        bool fDeleted;
        public bool Deleted
        {
            get { return fDeleted; }
            set { SetPropertyValue<bool>("Deleted", ref fDeleted, value); }
        }
        string fName;
        [Size(255)]
        public string Name
        {
            get { return fName; }
            set { SetPropertyValue<string>("Name", ref fName, value); }
        }
        string fCode;
        [Size(255)]
        public string Code
        {
            get { return fCode; }
            set { SetPropertyValue<string>("Code", ref fCode, value); }
        }
        int fDepartmentHeadId;
        public int DepartmentHeadId
        {
            get { return fDepartmentHeadId; }
            set { SetPropertyValue<int>("DepartmentHeadId", ref fDepartmentHeadId, value); }
        }
        int fDepartmentHeadDeputyId;
        public int DepartmentHeadDeputyId
        {
            get { return fDepartmentHeadDeputyId; }
            set { SetPropertyValue<int>("DepartmentHeadDeputyId", ref fDepartmentHeadDeputyId, value); }
        }
        int fParentId;
        public int ParentId
        {
            get { return fParentId; }
            set { SetPropertyValue<int>("ParentId", ref fParentId, value); }
        }
        int fType;
        public int Type
        {
            get { return fType; }
            set { SetPropertyValue<int>("Type", ref fType, value); }
        }
        int flft;
        public int lft
        {
            get { return flft; }
            set { SetPropertyValue<int>("lft", ref flft, value); }
        }
        int frgt;
        public int rgt
        {
            get { return frgt; }
            set { SetPropertyValue<int>("rgt", ref frgt, value); }
        }
        int fPosition;
        public int Position
        {
            get { return fPosition; }
            set { SetPropertyValue<int>("Position", ref fPosition, value); }
        }
        string fFullName;
        [Size(512)]
        public string FullName
        {
            get { return fFullName; }
            set { SetPropertyValue<string>("FullName", ref fFullName, value); }
        }
        int fDefaultPassageId;
        public int DefaultPassageId
        {
            get { return fDefaultPassageId; }
            set { SetPropertyValue<int>("DefaultPassageId", ref fDefaultPassageId, value); }
        }
        [Association(@"UsersReferencesDepartments")]
        public XPCollection<Users> UsersCollection { get { return GetCollection<Users>("UsersCollection"); } }
    }

}
