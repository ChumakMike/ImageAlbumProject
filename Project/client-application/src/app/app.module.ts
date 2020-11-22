import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from "@angular/forms";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
 
import { ToastrModule } from 'ngx-toastr';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { UserService } from './shared/user.service';
import { LoginComponent } from './user/login/login.component';
import { HomeComponent } from './home/home.component';
import { AuhtInterceptor } from './auth/auth.interceptor';
import { UsersComponent } from './home/users/users.component';
import { HomeProfileComponent } from './home/home-profile/home-profile.component';
import { RoleService } from './shared/role.service';
import { RolesComponent } from './home/roles/roles.component';
import { CategoriesComponent } from './home/categories/categories.component';
import { CategoryService } from './shared/category.service';

@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    RegistrationComponent,
    LoginComponent,
    HomeComponent,
    UsersComponent,
    HomeProfileComponent,
    RolesComponent,
    CategoriesComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ToastrModule.forRoot()
  ],
  providers: [UserService, {
    provide : HTTP_INTERCEPTORS,
    useClass : AuhtInterceptor,
    multi : true 
  }, 
  RoleService,
  CategoryService
],
  bootstrap: [AppComponent]
})
export class AppModule { }
