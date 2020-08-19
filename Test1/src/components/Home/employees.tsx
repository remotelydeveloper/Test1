import './home.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { confirmAlert } from 'react-confirm-alert';
import 'react-confirm-alert/src/react-confirm-alert.css';

import * as React from 'react';
import { Input } from '@servicestack/react';
import { client } from '../../shared';
import {
  EmployeeRequest,
  DeleteEmployeeRequest,
  CreateEmployeeRequest,
} from '../../shared/dtos';
import { EmployeeResponse } from '../../shared/dtos';
import { EmployeesRequest } from '../../shared/dtos';

import DataTable from 'react-data-table-component';
import DataTableExtensions from 'react-data-table-component-extensions';

import Modal from '../Modal/modal';

export interface EmployeesProps {}

export const Employees: React.FC<any> = (props: EmployeesProps) => {
  const [data, setData] = React.useState([]);
  const [loading, setLoading] = React.useState(false);
  const [totalRows, setTotalRows] = React.useState(0);
  const [perPage, setPerPage] = React.useState(10);
  const [currentPage, setCurrentPage] = React.useState(1);

  const [modal, setModal] = React.useState(false);
  const [id, setId] = React.useState('');
  const [name, setName] = React.useState('');
  const [department, setDepartment] = React.useState('');
  const [address, setAddress] = React.useState('');
  const [city, setCity] = React.useState('');
  const [country, setCountry] = React.useState('');
  const [formError, setFormError] = React.useState(false);

  const [modalInputName, setModalInputName] = React.useState('');

  const columns = [
    {
      name: 'Name',
      selector: 'name',
      sortable: true,
    },
    {
      name: 'Department',
      selector: 'department',
      sortable: true,
    },
    {
      name: 'Address',
      selector: 'address',
      sortable: true,
    },
    {
      name: 'City',
      selector: 'city',
      sortable: true,
    },
    {
      name: 'Country',
      selector: 'country',
      sortable: true,
    },
    {
      cell: (row) => (
        <div>
          <span onClick={handleEdit(row)} className='edit-btn btn'>
            <FontAwesomeIcon icon='user-edit' />
          </span>
          <span onClick={confirmDelete(row)} className='delete-btn btn'>
            <FontAwesomeIcon icon='user-times' />
          </span>
        </div>
      ),
    },
  ];

  const handleEdit = React.useCallback((row) => () => {
    const employee = data.find((e) => e.id === row.id);
    setId(employee.id);
    setName(employee.name);
    setDepartment(employee.department);
    setAddress(employee.address);
    setCity(employee.city);
    setCountry(employee.country);
    setModal(true);
  });

  const confirmDelete = React.useCallback((row) => () => {
    confirmAlert({
      title: 'Confirmation',
      message: 'Are you sure you want to delete this employee?',
      buttons: [
        {
          label: 'Yes',
          onClick: () => deleteEmployee(row.id),
        },
        {
          label: 'No',
        },
      ],
    });
  });

  const handleAdd = () => {
    setId('');
    setName('');
    setDepartment('');
    setAddress('');
    setCity('');
    setCountry('');
    setModal('');
    modalOpen();
  };

  async function deleteEmployee(id) {
    await client.delete(new DeleteEmployeeRequest({ id: id }));
    const data = await client.get(
      new EmployeesRequest({ page: currentPage, size: perPage })
    );

    const updatedEmployees = data.results.slice();
    updatedEmployees.splice(
      updatedEmployees.findIndex((e) => e.id === id),
      1
    );
    setData(updatedEmployees);
    setTotalRows(totalRows - 1);
  }

  async function getEmployeesList(page, size = perPage) {
    setLoading(true);
    const data = await client.get(
      new EmployeesRequest({ page: page, size: size })
    );
    setData(data.results);
    setTotalRows(data.total);
    setLoading(false);
  }

  async function saveEmployee() {
    if (name.trim() == '') {
      return setFormError(true);
    }
    const data = await client.post(
      new CreateEmployeeRequest({
        id: id,
        name: name,
        department: department,
        address: address,
        city: city,
        country: country,
      })
    );

    await getEmployeesList(currentPage, perPage);
    modalClose();
  }

  React.useEffect(() => {
    getEmployeesList(1);
  }, []);

  const handlePageChange = (page) => {
    getEmployeesList(page);
    setCurrentPage(page);
  };

  const handlePerRowsChange = async (newPerPage, page) => {
    getEmployeesList(page, newPerPage);
    setPerPage(newPerPage);
  };

  const modalOpen = () => {
    setModal(true);
  };

  const modalClose = () => {
    setModal(false);
  };

  const handleInputValidation = (e) => {
    if (e.target.value.trim() == '') {
      setFormError(true);
      return;
    }
    setFormError(false);
  };

  return (
    <div className='employee-view'>
      <button
        className='create-employee btn btn-primary btn-sm'
        onClick={() => handleAdd()}
      >
        Create Employee <FontAwesomeIcon icon='user-plus' />
      </button>
      <DataTable
        className='employees-table'
        title=''
        columns={columns}
        data={data}
        progressPending={loading}
        pagination
        paginationServer
        paginationTotalRows={totalRows}
        paginationDefaultPage={currentPage}
        onChangeRowsPerPage={handlePerRowsChange}
        onChangePage={handlePageChange}
        selectableRows={false}
        onSelectedRowsChange={({ selectedRows }) => console.log(selectedRows)}
      />
      <Modal show={modal}>
        <h2>{id == '' ? 'Create Employee' : 'Edit Employee'}</h2>
        <div className='form-group'>
          <label>Enter Name:</label>
          <input
            type='text'
            value={name}
            name='name'
            onChange={(e) => setName(e.target.value)}
            className='form-control'
            onBlur={handleInputValidation}
          />
          {formError && (
            <span className='error text-danger'>Name is required</span>
          )}
        </div>
        <div className='form-group'>
          <label>Enter Department:</label>
          <input
            type='text'
            value={department}
            name='department'
            onChange={(e) => setDepartment(e.target.value)}
            className='form-control'
          />
        </div>
        <div className='form-group'>
          <label>Enter Address:</label>
          <input
            type='text'
            value={address}
            name='address'
            onChange={(e) => setAddress(e.target.value)}
            className='form-control'
          />
        </div>
        <div className='form-group'>
          <label>Enter City:</label>
          <input
            type='text'
            value={city}
            name='city'
            onChange={(e) => setCity(e.target.value)}
            className='form-control'
          />
        </div>
        <div className='form-group'>
          <label>Enter Country:</label>
          <input
            type='text'
            value={country}
            name='country'
            onChange={(e) => setCountry(e.target.value)}
            className='form-control'
          />
        </div>
        <div className='form-group action-btn-group'>
          <button
            className='save btn btn-primary btn-sm'
            onClick={() => saveEmployee()}
            type='button'
          >
            Save
          </button>
          <button
            href='javascript:;'
            className='btn btn-secondary btn-sm'
            onClick={modalClose}
          >
            Close
          </button>
        </div>
      </Modal>
    </div>
  );
};
