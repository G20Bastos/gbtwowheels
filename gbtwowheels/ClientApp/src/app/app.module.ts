import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { RegisterComponent } from './register/register.component';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './access/login.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { MainAdminComponent } from './main/main-admin.component';
import { AuthInterceptor } from './services/auth.interceptor';
import { MotorcyclesService } from './services/motorcycles.service';
import { OrdersService } from './services/orders.service';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    RegisterComponent,
    LoginComponent,
    MainAdminComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
      timeOut: 3000,
      preventDuplicates: true,
    }),
    RouterModule.forRoot([
      { path: '', redirectTo: '/register', pathMatch: 'full' },
      { path: 'register', component: RegisterComponent },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'login', component: LoginComponent },
      { path: 'main-admin', component: MainAdminComponent }
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    MotorcyclesService,
    OrdersService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
