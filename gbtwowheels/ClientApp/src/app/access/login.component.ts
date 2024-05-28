import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  userEmail: string = '';
  userPassword: string = '';

  constructor(private http: HttpClient, private router: Router, private toastr: ToastrService) { }

  onSubmit() {
    const user = {
      userEmail: this.userEmail,
      userPassword: this.userPassword
    };

    this.http.post<any>('https://localhost/api/user/login', user)
      .subscribe(response => {
        this.toastr.success('Usuário logado com sucesso!', 'Sucesso');
        localStorage.setItem('authToken', response.data.tokenAccess);
        localStorage.setItem('userId', response.data.userId);
        localStorage.setItem('fullName', response.data.firstName + ' ' + response.data.lastName);
        localStorage.setItem('categoryLicense', response.data.categoryLicense);
        if (response.data.levelId == 1) {
          this.router.navigate(['/main-admin']);
        } else {
          this.router.navigate(['/main-user']);
        }
        
      }, error => {
        this.toastr.error('Erro ao fazer login', 'Erro');
      });
  }
}
