import { Component, Inject, ViewChild  } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Employee } from './employee';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';


@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css']
})

export class EmployeesComponent {
  public displayedColumns: string[] = ['employeeId', 'surname', 'name', 'middleName', 'employmentDate', 'position', 'companyId', 'delete'];
  public employees: Employee[];
  employee: Employee;



  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    this.http.get<Employee[]>(this.baseUrl + 'api/employee')
      .subscribe(result => {
        this.employees = result;
      }, error => console.error(error));
  
  }

  delete(e: Employee) {
    var url = this.baseUrl + "api/employee/" + e.employeeId;
    this.http.delete<Employee>(url).subscribe(result => {
      console.log("Employee " + e.name + " has been deleted.");

      window.location.reload();
    }, error => console.error(error));
  }
  
}
