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

    this.http.post<any>('https://localhost:7296/api/user/login', user)
      .subscribe(response => {
        this.toastr.success('Usuário logado com sucesso!', 'Sucesso');
        this.router.navigate(['/main-admin']);
      }, error => {
        this.toastr.error('Erro ao fazer login', 'Erro');
      });
  }
}
