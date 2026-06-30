import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // Your interception logic here (e.g., cloning request to add a token)
   const token = localStorage.getItem('token');
 if (token) {
 const cloned = req.clone({
 setHeaders: {
   Authorization: `Bearer ${token}`}
 });
 return next(cloned);
 }
  return next(req);
};
