<template>
  <!-- ======= Header ======= -->
  <header id="header" class="header fixed-top d-flex align-items-center" v-if="auth">

    <div class="d-flex align-items-center justify-content-between">
      <router-link to="/" class="logo d-flex align-items-center" style="text-decoration: none;">
        <span class="d-none d-lg-block">thegioithuoc.com</span>
      </router-link>
      <i class="bi bi-list toggle-sidebar-btn"></i>
    </div>

    <nav class="header-nav ms-auto">
      <ul class="d-flex align-items-center">

        <li class="nav-item d-block d-lg-none">
          <a class="nav-link nav-icon search-bar-toggle " href="#">
            <i class="bi bi-search"></i>
          </a>
        </li>

        <li class="nav-item dropdown">

          <a class="nav-link nav-icon" href="#" data-bs-toggle="dropdown">
            <i class="bi bi-bell"></i>
            <span class="badge bg-primary badge-number">4</span>
          </a>

          <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow notifications">
            <li class="dropdown-header">
              You have 4 new notifications
              <a href="#"><span class="badge rounded-pill bg-primary p-2 ms-2">View all</span></a>
            </li>
            <li>
              <hr class="dropdown-divider">
            </li>

            <li class="notification-item">
              <i class="bi bi-exclamation-circle text-warning"></i>
              <div>
                <h4>Lorem Ipsum</h4>
                <p>Quae dolorem earum veritatis oditseno</p>
                <p>30 min. ago</p>
              </div>
            </li>

            <li>
              <hr class="dropdown-divider">
            </li>

            <li class="notification-item">
              <i class="bi bi-x-circle text-danger"></i>
              <div>
                <h4>Atque rerum nesciunt</h4>
                <p>Quae dolorem earum veritatis oditseno</p>
                <p>1 hr. ago</p>
              </div>
            </li>

            <li>
              <hr class="dropdown-divider">
            </li>

            <li class="notification-item">
              <i class="bi bi-check-circle text-success"></i>
              <div>
                <h4>Sit rerum fuga</h4>
                <p>Quae dolorem earum veritatis oditseno</p>
                <p>2 hrs. ago</p>
              </div>
            </li>

            <li>
              <hr class="dropdown-divider">
            </li>

            <li class="notification-item">
              <i class="bi bi-info-circle text-primary"></i>
              <div>
                <h4>Dicta reprehenderit</h4>
                <p>Quae dolorem earum veritatis oditseno</p>
                <p>4 hrs. ago</p>
              </div>
            </li>

            <li>
              <hr class="dropdown-divider">
            </li>
            <li class="dropdown-footer">
              <a href="#">Show all notifications</a>
            </li>

          </ul><!-- End Notification Dropdown Items -->

        </li>

        <li class="nav-item dropdown">

          <a class="nav-link nav-icon" href="#" data-bs-toggle="dropdown">
            <i class="bi bi-chat-left-text"></i>
            <span class="badge bg-success badge-number">3</span>
          </a>

          <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow messages">
            <li class="dropdown-header">
              You have 3 new messages
              <a href="#"><span class="badge rounded-pill bg-primary p-2 ms-2">View all</span></a>
            </li>
            <li>
              <hr class="dropdown-divider">
            </li>

            <li class="message-item">
              <a href="#">
                <img src="" alt="" class="rounded-circle">
                <div>
                  <h4>Maria Hudson</h4>
                  <p>Velit asperiores et ducimus soluta repudiandae labore officia est ut...</p>
                  <p>4 hrs. ago</p>
                </div>
              </a>
            </li>
            <li>
              <hr class="dropdown-divider">
            </li>

            <li class="message-item">
              <a href="#">
                <img src="" alt="" class="rounded-circle">
                <div>
                  <h4>Anna Nelson</h4>
                  <p>Velit asperiores et ducimus soluta repudiandae labore officia est ut...</p>
                  <p>6 hrs. ago</p>
                </div>
              </a>
            </li>
            <li>
              <hr class="dropdown-divider">
            </li>

            <li class="message-item">
              <a href="#">
                <img src="" alt="" class="rounded-circle">
                <div>
                  <h4>David Muldon</h4>
                  <p>Velit asperiores et ducimus soluta repudiandae labore officia est ut...</p>
                  <p>8 hrs. ago</p>
                </div>
              </a>
            </li>
            <li>
              <hr class="dropdown-divider">
            </li>

            <li class="dropdown-footer">
              <a href="#">Show all messages</a>
            </li>

          </ul>

        </li>

        <li class="nav-item dropdown pe-3">

          <a class="nav-link nav-profile d-flex align-items-center pe-0" href="#" data-bs-toggle="dropdown">
            <i class="bi bi-person"></i>
            <span class="d-none d-md-block dropdown-toggle ps-2">Quản trị viên</span>
          </a>

          <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow profile">
<!--            <li class="dropdown-header">-->
<!--              <h6>Kevin Anderson</h6>-->
<!--              <span>Web Designer</span>-->
<!--            </li>-->
<!--            <li>-->
<!--              <hr class="dropdown-divider">-->
<!--            </li>-->

            <li>
              <a class="dropdown-item d-flex align-items-center" href="" style="text-decoration: none">
                <i class="bi bi-person"></i>
                <span>Thông tin cá nhân</span>
              </a>
            </li>
            <li>
              <hr class="dropdown-divider">
            </li>

<!--            <li>-->
<!--              <a class="dropdown-item d-flex align-items-center" href="users-profile.html">-->
<!--                <i class="bi bi-gear"></i>-->
<!--                <span>Account Settings</span>-->
<!--              </a>-->
<!--            </li>-->
<!--            <li>-->
<!--              <hr class="dropdown-divider">-->
<!--            </li>-->

<!--            <li>-->
<!--              <a class="dropdown-item d-flex align-items-center" href="pages-faq.html">-->
<!--                <i class="bi bi-question-circle"></i>-->
<!--                <span>Need Help?</span>-->
<!--              </a>-->
<!--            </li>-->
<!--            <li>-->
<!--              <hr class="dropdown-divider">-->
<!--            </li>-->

            <li>
              <router-link to="/" @click="logout" class="dropdown-item d-flex align-items-center" style="text-decoration: none">
                <i class="bi bi-box-arrow-right"></i>
                <span>Đăng xuất</span>
              </router-link>
            </li>

          </ul>
        </li>

      </ul>
    </nav>

  </header>

  <!-- ======= Sidebar ======= -->
  <aside id="sidebar" class="sidebar" v-if="auth">

    <ul class="sidebar-nav" id="sidebar-nav">

      <li class="nav-item">
        <a class="nav-link collapsed" data-bs-target="#home-nav" data-bs-toggle="collapse" href="#">
          <i class="bi bi-grid"></i><span>Trang chủ</span><i class="bi bi-chevron-down ms-auto"></i>
        </a>
        <ul id="home-nav" class="nav-content collapse " data-bs-parent="#sidebar-nav">
          <li>
            <router-link to="/" style="text-decoration: none">
              <i class="bi bi-circle"></i>
              <span>Thống kê</span>
            </router-link>
          </li>
        </ul>
      </li>
      <li class="nav-item">
        <a class="nav-link collapsed" data-bs-target="#components-nav" data-bs-toggle="collapse" href="#">
          <i class="bi bi-menu-button-wide"></i><span>Quản lý danh mục</span><i class="bi bi-chevron-down ms-auto"></i>
        </a>
        <ul id="components-nav" class="nav-content collapse " data-bs-parent="#sidebar-nav">
          <li>
            <router-link to="/categories" style="text-decoration: none">
              <i class="bi bi-circle"></i><span>Danh sách danh mục</span>
            </router-link>
          </li>
          <li>
            <router-link to="/categories/create" style="text-decoration: none">
              <i class="bi bi-circle"></i><span>Thêm danh mục</span>
            </router-link>
          </li>
        </ul>
      </li>

      <li class="nav-item">
        <a class="nav-link collapsed" data-bs-target="#forms-nav" data-bs-toggle="collapse" href="#">
          <i class="bi bi-journal-text"></i><span>Quản lý nhà thuốc</span><i class="bi bi-chevron-down ms-auto"></i>
        </a>
        <ul id="forms-nav" class="nav-content collapse " data-bs-parent="#sidebar-nav">
          <li>
            <router-link to="/stores" style="text-decoration: none">
              <i class="bi bi-circle"></i><span>Tổng quan</span>
            </router-link>
          </li>
          <li>
            <router-link to="/stores/my-store" style="text-decoration: none">
              <i class="bi bi-circle"></i><span>Danh sách nhà thuốc của tôi</span>
            </router-link>
          </li>
          <li>
            <router-link to="/stores/create" style="text-decoration: none">
              <i class="bi bi-circle"></i><span>Thêm nhà thuốc</span>
            </router-link>
          </li>
          <li>
            <router-link to="/stores/list-store" style="text-decoration: none">
              <i class="bi bi-circle"></i><span>Danh sách nhà thuốc cá nhân</span>
            </router-link>
          </li>
        </ul>
      </li>

      <li class="nav-item">
        <a class="nav-link collapsed" data-bs-target="#tables-nav" data-bs-toggle="collapse" href="#">
          <i class="bi bi-layout-text-window-reverse"></i><span>Quản lý sản phẩm</span><i class="bi bi-chevron-down ms-auto"></i>
        </a>
        <ul id="tables-nav" class="nav-content collapse " data-bs-parent="#sidebar-nav">
          <li>
            <router-link to="/products"  style="text-decoration: none">
              <i class="bi bi-circle"></i><span>Danh sách sản phẩm</span>
            </router-link>
          </li>
          <li>
            <a href="" style="text-decoration: none">
              <i class="bi bi-circle"></i><span>Duyệt sản phẩm</span>
            </a>
          </li>
          <li>
            <router-link to="/uses" style="text-decoration: none">
              <i class="bi bi-circle"></i><span>Danh sách công dụng </span>
            </router-link>
          </li>
          <li>
            <router-link to="/contraindications" style="text-decoration: none">
              <i class="bi bi-circle"></i><span>Danh sách chống chỉ định </span>
            </router-link>
          </li>
          <li>
            <router-link to="/manufacturers" style="text-decoration: none">
              <i class="bi bi-circle"></i><span>Danh sách nhà xuất bản </span>
            </router-link>
          </li>
          <li>
            <router-link to="/dosage-forms" style="text-decoration: none">
              <i class="bi bi-circle"></i><span>Danh sách dạng bào chế </span>
            </router-link>
          </li>
        </ul>
      </li>

      <li class="nav-item">
        <a class="nav-link collapsed" data-bs-target="#charts-nav" data-bs-toggle="collapse" href="#">
          <i class="bi bi-bar-chart"></i><span>Quản lý chương trình khuyến mãi</span><i class="bi bi-chevron-down ms-auto"></i>
        </a>
        <ul id="charts-nav" class="nav-content collapse " data-bs-parent="#sidebar-nav">
          <li>
            <router-link to="/vouchers" style="text-decoration: none">
              <i class="bi bi-circle"></i><span>Danh sách</span>
            </router-link>
          </li>
          <li>
            <router-link to="/vouchers/create" style="text-decoration: none">
              <i class="bi bi-circle"></i><span>Thêm khuyến mãi</span>
            </router-link>
          </li>
        </ul>
      </li>

      <li class="nav-item">
        <a class="nav-link collapsed" data-bs-target="#icons-nav" data-bs-toggle="collapse" href="#">
          <i class="bi bi-gem"></i><span>Quản lý tài khoản người dùng</span><i class="bi bi-chevron-down ms-auto"></i>
        </a>
        <ul id="icons-nav" class="nav-content collapse " data-bs-parent="#sidebar-nav">
          <li>
            <router-link to="/accounts" style="text-decoration: none">
              <i class="bi bi-circle"></i><span>Danh sách</span>
            </router-link>
          </li>
          <li>
            <a href="">
              <i class="bi bi-circle"></i><span>Quyền truy cập</span>
            </a>
          </li>
          <li>
            <a href="">
              <i class="bi bi-circle"></i><span>Thêm tài khoản</span>
            </a>
          </li>
        </ul>
      </li>
    </ul>

  </aside>
</template>
<script>
import { useToast } from 'vue-toastification'
import {useStore} from "vuex";
import {computed} from "vue";
import {useRouter} from "vue-router";
import axios from "axios";
import AuthService from "@/services/auth.service.js";
export default {
  name: "NavbarComponent",
  setup(){
    const toast = useToast()
    const store = useStore();
    const router = useRouter();
    const auth = computed(() => store.state.auth);
    const message = computed(() => store.state.message);
    const logout = async () =>{
      await AuthService.logout().then(async response => {
        sessionStorage.removeItem('token');
        axios.defaults.headers.common['Authorization'] = '';
        await store.dispatch('setMessage','')
        await store.dispatch('setAuth',false);
        toast.success("Đăng xuất thành công.")
        await router.push('/login');
      }).catch(error => {
        console.log(error.response.data)
      })
    }
    return {
      auth,
      logout,
      message
    }
  },
}
</script>
<style scoped>

</style>