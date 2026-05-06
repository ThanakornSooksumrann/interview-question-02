import { environment } from '../../../environments/environment';

export const API_BASE_URL = environment.apiBaseUrl;

export const API_ENDPOINTS = {
  AUTH: {
    REGISTER: '/api/Auth/register',
    LOGIN: '/api/Auth/login',
    ME: '/api/Auth/me',
    LOGOUT: '/api/Auth/logout',
  },
} as const;

export const STORAGE_KEYS = {
  ACCESS_TOKEN: 'access_token',
  REFRESH_TOKEN: 'refresh_token',
  USERNAME: 'username',
} as const;

export const API_STATUS_CODES = {
  SUCCESS: 200,
  ERROR_BUSINESS: 400,
  UNAUTHORIZED: 401,
  NOT_FOUND: 404,
  EXPIRE_ACCESS_TOKEN: 419,
  INTERNAL_SERVER_ERROR: 500,
} as const;
