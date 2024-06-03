import React, { useState } from 'react';
import './App.css';

function App() {
  const [employees, setEmployees] = useState([]);
  const [field, setField] = useState('');
  const [filter, setFilter] = useState('');
  const [value, setValue] = useState('');
  const [sort, setSort] = useState('');

  const fetchData = async (queryString) => {
    try {
      const response = await fetch(queryString);
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      const data = await response.json();
      setEmployees(data);
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    const queryString = `https://localhost:7248/api/employee?field=${field}&filter=${filter}&value=${value}&sort=${sort}`;
    fetchData(queryString);
  };

  return (
    <div className="App">
      <h1>Employee Table</h1>
      <form onSubmit={handleSubmit}>
        <div className='container'>
        <div>
          <label>
            Field:
            <input type="text" value={field} onChange={(e) => setField(e.target.value)} />
            <span>
              EmployeeId / When
            </span>
          </label>
        </div>
        <div>
          <label>
            Filter:
            <input type="text" value={filter} onChange={(e) => setFilter(e.target.value)} />
            <span>
              Equals / LessThan / GreaterThan
            </span>
          </label>
        </div>
        <div>
          <label>
            Value:
            <input type="text" value={value} onChange={(e) => setValue(e.target.value)} />
            <span>
              Number / Date (yyyy-MM-ddThh:mm:ssZ)
            </span>
          </label>
        </div>
        <div>
          <label>
            Sort:
            <input type="text" value={sort} onChange={(e) => setSort(e.target.value)} />
            <span>
              Ascending / Descending
            </span>
          </label>
        </div>
        <button type="submit">Fetch Data</button>
        </div>
      </form>
      <table>
        <thead>
          <tr>
            <th>Employee ID</th>
            <th>When</th>
          </tr>
        </thead>
        <tbody>
          {employees.map((employee) => (
            <tr key={employee.employeeId}>
              <td>{employee.employeeId}</td>
              <td>{employee.when}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default App;