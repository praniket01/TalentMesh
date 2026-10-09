import { isPlatformBrowser } from "@angular/common";
import { HttpInterceptorFn } from "@angular/common/http";
import { inject, PLATFORM_ID } from "@angular/core";
import { AuthService } from "../services/auth.service";

export const authInterceptor: HttpInterceptorFn =
    (req, next) => {
        const authService = inject(AuthService);
        const token = authService.getToken();

        if (!token) {
            return next(req);
        }

        const authReq = req.clone({
            setHeaders: {
                Authorization: `Bearer ${token}`
            }
        });

        return next(authReq);
    }