import { Component, Inject, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Company } from './Company';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';


@Component({
  selector: 'app-companies',
  templateUrl: './companies.component.html',
  styleUrls: ['./companies.component.css']
})

export class CompaniesComponent {
  public displayedColumns: string[] = ['companyId', 'name', 'legalForm', 'delete'];
  public companies: Company[];
  company: Company;



  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    this.http.get<Company[]>(this.baseUrl + 'api/company')
      .subscribe(result => {
        this.companies = result;
      }, error => console.error(error));

  }

  delete(c: Company) {
    var url = this.baseUrl + "api/company/" + c.companyId;
    this.http.delete<Company>(url).subscribe(result => {
      console.log("Employee " + c.name + " has been deleted.");

      window.location.reload();
    }, error => console.error(error));
  }

}
