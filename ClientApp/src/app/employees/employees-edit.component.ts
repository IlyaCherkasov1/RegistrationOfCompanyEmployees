import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, FormControl, Validators, AbstractControl, AsyncValidatorFn } from '@angular/forms';

import { Employee } from './employee';
import { Company } from '../companies/company';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-employees-edit',
  templateUrl: './employees-edit.component.html',
  styleUrls: ['./employees-edit.component.css']
})
export class EmployeeEditComponent {

  title: string;

  form: FormGroup;

  employee: Employee;

  companies: Company[];

  id?: number;

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {

    this.form = this.fb.group({
      surname:
        ['',
          Validators.required,
        ],

      name: ['',
        
          Validators.required
      ],

      middleName: ['',
          Validators.required, 
      ],

      employmentDate: ['',
        
          Validators.required
   
      ],

      position: ['',
        [
          Validators.required,
          Validators.pattern('[a-zA-Z]+')],
      ],

      companyId: ['',
        Validators.required,
      ],

      });
    this.loadData();
  }
  
  loadData() {

    this.loadCompanies();
     this.id = +this.activatedRoute.snapshot.paramMap.get('id');

    if (this.id) {

      var url = this.baseUrl + "api/employee/" + this.id;
      this.http.get<Employee>(url).subscribe(result => {
        this.employee = result;
        this.title = "Edit - " + this.employee.name;

         this.form.patchValue(this.employee);
      }, error => console.error(error));
    }
    else {
      this.title = "Create a new Employee";
    }    
  }
  

  loadCompanies() {
    var url = this.baseUrl + "api/company";

    this.http.get<any>(url).subscribe(result =>
    {
      this.companies = result;
    }, error => console.error(error));
  }


  onSubmit() {

    var employee = (this.id) ? this.employee : <Employee>{};
 
    employee.surname = this.form.get("surname").value;
    employee.name = this.form.get("name").value;
    employee.middleName = this.form.get("middleName").value.toString();
    employee.employmentDate = this.form.get("employmentDate").value;
    employee.position = this.form.get("position").value;
    employee.companyId = +this.form.get("companyId").value;


    if (this.id) {
      var url = this.baseUrl + "api/employee/" + this.employee.employeeId;
      this.http
        .put<Employee>(url, employee)
        .subscribe(result => {

          console.log("Employee " + employee.name + " has been updated.");

          this.router.navigate(['/employees']);
        }, error => console.error(error));
    }

    else {
      var url = this.baseUrl + "api/employee";
      this.http
        .post<Employee>(url, employee)
        .subscribe(result => {
          console.log("Employee" + employee.name + "has been created");
          this.router.navigate(['/employees']);
        }, error => console.error(error));
    }
  }

  isDupeField(fieldName: string): AsyncValidatorFn {
    return (control: AbstractControl): Observable<{
      [key: string]: any
    } | null> => {
      var params = new HttpParams().set("contryId",
        (this.id) ? this.id.toString() : "0").set("fieldName", fieldName).
        set("fieldValue", control.value); var url = this.baseUrl + "api/employee/IsDupeField";
      return this.http.post<boolean>(url, null, { params }).pipe(map(result => {
        return (result ?
          { isDupeField: true } : null);
      }));
    }
  } 
}
