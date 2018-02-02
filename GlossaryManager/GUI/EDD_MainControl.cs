﻿using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace GlossaryManager.GUI
{
	/// <summary>
	/// Description of EDD_MainControl.
	/// </summary>
	public partial class EDD_MainControl : UserControl
	{
        public List<Domain> domains { get; set; }
        public EDD_MainControl()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		public void setBusinessItems(IEnumerable<BusinessItem> businessItems)
		{
			this.BusinessItemsListView.Objects = businessItems;
		}
        public void SetDomains(List<Domain> domains)
        {
            this.domains = domains;
            BU_DomainComboBox.DataSource = this.domains;
            BU_DomainComboBox.DisplayMember = "displayName";
            //set the domains breadcrumb
            foreach (var domain in domains)
            {
                if (domain.parentDomain == null) //only process top level domains
                {
                    domainBreadCrumb.RootItem.Items.Add(createDomainBreadCrumbItem(domain));
                }
            }
        }
        public KryptonBreadCrumbItem createDomainBreadCrumbItem(Domain domain)
        {
            var breadCrumbItem = new KryptonBreadCrumbItem(domain.name);
            breadCrumbItem.Tag = domain;
            foreach(var subDomain in domain.subDomains)
            {
                breadCrumbItem.Items.Add(createDomainBreadCrumbItem(subDomain));
            }
            return breadCrumbItem;
        }
        private BusinessItem selectedBusinessItem
		{
			get
			{
				return this.BusinessItemsListView.SelectedObject as BusinessItem;
			}
		}

        private void BusinessItemsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadSelectedItemData();
        }

        private void loadSelectedItemData()
        {
            if (selectedBusinessItem != null)
            {
                this.BU_NameTextBox.Text = selectedBusinessItem.Name;
                this.BU_DomainComboBox.SelectedItem = selectedBusinessItem.domain;
                this.BU_DescriptionTextBox.Text = selectedBusinessItem.Description;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (selectedBusinessItem != null)
            {
                selectedBusinessItem.Name = this.BU_NameTextBox.Text;
                selectedBusinessItem.domain = (Domain)this.BU_DomainComboBox.SelectedItem;
                selectedBusinessItem.Description = this.BU_DescriptionTextBox.Text;
                selectedBusinessItem.Save();
                //refresh listview
                this.BusinessItemsListView.RefreshSelectedObjects();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            loadSelectedItemData();
        }

        public event EventHandler selectedDomainChanged;
        private void domainBreadCrumb_SelectedItemChanged(object sender, EventArgs e)
        {
            var selectedDomain = domainBreadCrumb.SelectedItem.Tag as Domain;
            this.selectedDomainChanged?.Invoke(selectedDomain, e);
        }
    }
}