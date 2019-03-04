using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using MVCWeb.Cores.Entities;
using MVCWeb.Cores.IRepositories;
using MVCWeb.Cores.IServices;

namespace MVCWeb.Cores.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(
            ICustomerRepository customerRepository
            )
        {
            _customerRepository = customerRepository;
        }

        public int Create(Customer customer)
        {
            customer.CreatedOn = DateTime.Now;
            _customerRepository.Insert(customer);
            return customer.Id;
        }
        public bool UpdateCustomer(Customer customer)
        {
            var currentCustomer = _customerRepository.GetById(customer.Id);
            if (currentCustomer == null) return false;
            currentCustomer.CustomerName = customer.CustomerName;
            currentCustomer.PhoneNo = customer.PhoneNo;
            currentCustomer.Email = customer.Email;
            currentCustomer.Address = customer.Address;
            currentCustomer.Region = customer.Region;
            currentCustomer.Area = customer.Area;
            currentCustomer.Note = customer.Note;
            currentCustomer.IsVIP = customer.IsVIP;
            _customerRepository.Update(customer);
            return true;
        }
        public List<Customer> GetList(FilterParams fp, ref int totalCount)
        {
            var list = _customerRepository.TableNoTracking;
            if (!string.IsNullOrEmpty(fp.Keyword))
            {
                list = list.Where(o => o.CustomerName.Contains(fp.Keyword) || o.Email.Contains(fp.Keyword) || o.PhoneNo.Contains(fp.Keyword));
            }
            if (fp.IsVIP != 0)
            {
                list = list.Where(o => o.IsVIP);
            }
            totalCount = list.Count();
            list = list.OrderBy(fp.SortField + (fp.SortASC ? " ASC" : " DESC"));
            if (fp.PageNumber == 0) return list.ToList();
            var skip = (fp.PageNumber - 1) * fp.PageSize;
            var take = fp.PageSize;
            list = list.Skip(skip).Take(take);
            return list.ToList();
        }
    }
}