import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { AvatarModule } from 'primeng/avatar';
import { CardModule } from 'primeng/card';
import { TagModule } from 'primeng/tag';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-welcome',
  standalone: true,
  imports: [CommonModule, ButtonModule, AvatarModule, CardModule, TagModule],
  templateUrl: './welcome.component.html',
})
export class WelcomeComponent {
  readonly authService = inject(AuthService);

  get username(): string   { return this.authService.currentUsername() ?? ''; }
  get initial(): string    { return this.username.charAt(0).toUpperCase(); }

  onLogout(): void { this.authService.logout(); }
}
