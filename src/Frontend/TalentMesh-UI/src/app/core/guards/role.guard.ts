import { inject } from "@angular/core"
import { AuthService } from "../services/auth.service"
import { CanActivate, CanActivateFn, Router } from "@angular/router";

export const roleGuard : CanActivateFn = (route, state) => {
    const authService = inject(AuthService);
    const router = inject(Router);

    if (!authService.isAuthenticated()) {
        return router.createUrlTree(['/login'], {
            queryParams: { returnUrl: state.url }
        });
    }
    const allowedRoles = route.data['roles'] as string[] | undefined;
    const currentRole = authService.userRole();

    if (!allowedRoles || (currentRole && allowedRoles.includes(currentRole))) {
        return true;
    }

    return router.createUrlTree(['/dashboard']);
}