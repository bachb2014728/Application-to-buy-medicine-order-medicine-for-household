import axios from 'axios';
import toast from "bootstrap/js/src/toast.js";
const commonConfig = {
    headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
    },
};
export default function createInstance(baseURL) {
    const instance = axios.create({
        baseURL,
        ...commonConfig
        // Cấu hình axios instance ở đây (ví dụ: timeout, headers, ...)
    });

    instance.interceptors.response.use(
        response => {
            // Xử lý response thành công ở đây
            return response;
        },
        error => {
            // Xử lý các lỗi HTTP ở đây
            if (error.response) {
                // Server trả về một response với trạng thái lỗi
                if (error.response.status === 404) {
                    // Xử lý lỗi 404 cụ thể ở đây
                    toast.error('Không tìm thấy trang hoặc tài nguyên yêu cầu.');
                } else {
                    // Xử lý các lỗi HTTP khác
                    toast.error(error.response.data.detail || 'Có lỗi xảy ra khi đăng nhập.');
                }
            } else if (error.request) {
                // Yêu cầu đã được thực hiện nhưng không nhận được phản hồi
                toast.error('Không nhận được phản hồi từ máy chủ.');
            } else {
                // Lỗi được thiết lập khi thiết lập yêu cầu
                toast.error('Có lỗi xảy ra khi thiết lập yêu cầu: ' + error.message);
            }
            return Promise.reject(error);
        }
    );

    return instance;
}

