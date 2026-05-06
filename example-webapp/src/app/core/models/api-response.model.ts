export interface ApiResponse {
  code: number;
  message: string;
}

export interface ApiResponseWithData<T> extends ApiResponse {
  data: T;
}
