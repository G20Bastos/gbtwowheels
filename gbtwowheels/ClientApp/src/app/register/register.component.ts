import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

interface User {
  firstName: string;
  lastName: string;
  cnpj: string;
  dateOfBirth: Date;
  categoryLicense: string;
  licenseNumber: number;
  imageLicense: File | null;
  userEmail: string;
  userPassword: string;
}

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  user: User = {
    firstName: '',
    lastName: '',
    cnpj: '',
    dateOfBirth: new Date(),
    categoryLicense: '',
    licenseNumber: 0,
    imageLicense: null,
    userEmail: '',
    userPassword: ''
  };

  constructor(private http: HttpClient, private router: Router) { }

  onFileChange(event: any) {
    if (event.target.files.length > 0) {
      this.user.imageLicense = event.target.files[0];
    }
  }

  onSubmit() {
    const formData = new FormData();
    formData.append('firstName', this.user.firstName);
    formData.append('lastName', this.user.lastName);
    formData.append('cnpj', this.user.cnpj);
    formData.append('dateOfBirth', this.user.dateOfBirth.toISOString());
    formData.append('categoryLicense', this.user.categoryLicense);
    formData.append('licenseNumber', this.user.licenseNumber.toString());
    if (this.user.imageLicense) {
      formData.append('imageLicense', this.user.imageLicense);
    }
    formData.append('userEmail', this.user.userEmail);
    formData.append('userPassword', this.user.userPassword);

    this.http.post('http://localhost:5000/api/user', formData)
      .subscribe(response => {
        console.log('User registered successfully', response);
        this.router.navigate(['/']);
      }, error => {
        console.error('Error registering user', error);
      });
  }
}
