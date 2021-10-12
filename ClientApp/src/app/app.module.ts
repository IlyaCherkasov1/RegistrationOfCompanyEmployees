import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { EmployeesComponent } from './employees/employees.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AngularMaterialModule } from './angular-material.module';
import { MatInputModule } from '@angular/material/input';
import { CompaniesComponent } from './companies/companies.component';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { EmployeeEditComponent } from './employees/employees-edit.component';
import { CompanyEditComponent } from './companies/companies-edit.component';




@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    EmployeesComponent,
    EmployeeEditComponent,
    CompanyEditComponent,
    
    CompaniesComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AngularMaterialModule, 
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'employees', component: EmployeesComponent },
      { path: 'companies', component: CompaniesComponent },
      { path: 'employee/:id', component: EmployeeEditComponent },
      { path: 'employee', component: EmployeeEditComponent },
      { path: 'company/:id', component: CompanyEditComponent },
      { path: 'company', component: CompanyEditComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      
    ]),
    BrowserAnimationsModule,
    AngularMaterialModule,
    MatInputModule,
    ReactiveFormsModule
  ],
  exports: [
    MatInputModule,

  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
