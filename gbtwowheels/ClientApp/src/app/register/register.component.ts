import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { User } from '../model/user.model';
import { UsersService } from '../services/users.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit{


  user: User = this.createEmptyUser();


  licenseCategories = [
    { key: 'A', value: 'A' },
    { key: 'B', value: 'B' },
    { key: 'AB', value: 'AB' }
  ];

  constructor(private http: HttpClient, private router: Router, private toastr: ToastrService, private usersService: UsersService) { }

  ngOnInit() {

    
  }

  onFileChange(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      const fileType = file.type;

      if (fileType === 'image/png' || fileType === 'image/bmp') {
        this.user.imageFile = file;
      } else {
        event.target.value = '';
        this.user.imageFile = null;
        this.toastr.error('Favor, insira arquivos apenas com os formatos: PNG, BMP');
      }
    }

   }

  onSubmit() {

    if (this.user.firstName && this.user.lastName && this.user.cnpj && this.user.dateOfBirth && this.user.categoryLicense, this.user.licenseNumber && this.user.imageFile, this.user.userEmail, this.user.userPassword) {

      const formData = new FormData();
      const dateOfBirth = new Date(this.user.dateOfBirth);
      if (!isNaN(dateOfBirth.getTime())) {
        const today = new Date();
        let age = today.getFullYear() - dateOfBirth.getFullYear();
        const monthDifference = today.getMonth() - dateOfBirth.getMonth();
        const dayDifference = today.getDate() - dateOfBirth.getDate();

        if (monthDifference < 0 || (monthDifference === 0 && dayDifference < 0)) {
          age--;
        }

        if (age < 18) {
          this.toastr.error('É necessário ter 18 anos ou mais');
          return;
        }
      } else {
        this.toastr.error('Data de nascimento inválida');
        return;
      }


      if (this.user.imageFile) {

        this.usersService.createUser(this.user, this.user.imageFile).subscribe({
          next: (response) => {
            if (response.success) {
              this.toastr.success(response.message);
              this.resetForm();
              this.router.navigate(['/login']);
            } else {
              this.toastr.error(response.message);
            }
          },
          error: (error) => {
            if (error.error && error.error.message) {
              this.toastr.error(error.error.message);
            } else {
              this.toastr.error('Ocorreu um erro ao cadastrar o usuário.');
            }
            console.error(error);
          }
        });
      }


    } else {
      this.toastr.error('Preencha todos os campos');
    }
    
  }

  createEmptyUser(): User {
    return {
      firstName: '',
      lastName: '',
      cnpj: '',
      dateOfBirth: new Date(),
      categoryLicense: 'A',
      licenseNumber: 0,
      imageFile: null,
      userEmail: '',
      userPassword: '',
      levelId: 3
    };
  }

  applyCnpjMask(event: any) {
    const cnpj = event.target.value.replace(/\D/g, '');
    let maskedCnpj = '';

    if (cnpj.length <= 14) {
      maskedCnpj = cnpj.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})$/, '$1.$2.$3/$4-$5');
    } else {
      maskedCnpj = cnpj.slice(0, 14).replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})$/, '$1.$2.$3/$4-$5');
    }

    event.target.value = maskedCnpj;
  }


  resetForm() {
    this.user = {
      firstName: '',
      lastName: '',
      cnpj: '',
      dateOfBirth: new Date(),
      categoryLicense: 'A',
      licenseNumber: 0,
      imageFile: null,
      userEmail: '',
      userPassword: '',
      levelId: 3
    };
    const fileInput = document.getElementById('imageFile') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  }

}
