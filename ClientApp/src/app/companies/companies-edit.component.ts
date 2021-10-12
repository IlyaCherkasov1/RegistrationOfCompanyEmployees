import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, FormControl, Validators, AbstractControl, AsyncValidatorFn } from '@angular/forms';

import { Company } from './company';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-companies-edit',
  templateUrl: './companies-edit.component.html',
  styleUrls: ['./companies-edit.component.css']
})
export class CompanyEditComponent {

  title: string;

  form: FormGroup;

  companies: Company[];

  company: Company;

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
      companyId:
        ['',
          Validators.required,
        ],

      name: ['',
        Validators.required
      ],

      legalForm: ['',
        Validators.required,
      ],
    });
    this.loadData();
  }

  loadData() {

    this.id = +this.activatedRoute.snapshot.paramMap.get('id');

    if (this.id) {

      var url = this.baseUrl + "api/company/" + this.id;
      this.http.get<Company>(url).subscribe(result => {
        this.company = result;
        this.title = "Edit - " + this.company.name;

        this.form.patchValue(this.company);
      }, error => console.error(error));
    }
    else {
      this.title = "Create a new Company";
    }
  }


  onSubmit() {

    var company = (this.id) ? this.company : <Company>{};

    company.companyId = this.form.get("companyId").value;
    company.name = this.form.get("name").value;
    company.legalForm = this.form.get("legalForm").value.toString();
    
    if (this.id) {
      var url = this.baseUrl + "api/company/" + this.company.companyId;
      this.http
        .put<Company>(url, company)
        .subscribe(result => {

          console.log("Company " + company.name + " has been updated.");

          this.router.navigate(['/companies']);
        }, error => console.error(error));
    }

    else {
      var url = this.baseUrl + "api/company";
      this.http
        .post<Company>(url, company)
        .subscribe(result => {
          console.log("Employee" + company.name + "has been created");
          this.router.navigate(['/companies']);
        }, error => console.error(error));
    }
  }

}
