<template>
  <svg xmlns="http://www.w3.org/2000/svg" style="display: none;">
    <defs>
      <symbol id="facebook" viewBox="0 0 24 24">
        <path fill="currentColor"
              d="M12 2.04c-5.5 0-10 4.49-10 10.02c0 5 3.66 9.15 8.44 9.9v-7H7.9v-2.9h2.54V9.85c0-2.51 1.49-3.89 3.78-3.89c1.09 0 2.23.19 2.23.19v2.47h-1.26c-1.24 0-1.63.77-1.63 1.56v1.88h2.78l-.45 2.9h-2.33v7a10 10 0 0 0 8.44-9.9c0-5.53-4.5-10.02-10-10.02" />
      </symbol>
      <symbol id="youtube" viewBox="0 0 24 24">
        <path fill="currentColor"
              d="m10 15l5.19-3L10 9zm11.56-7.83c.13.47.22 1.1.28 1.9c.07.8.1 1.49.1 2.09L22 12c0 2.19-.16 3.8-.44 4.83c-.25.9-.83 1.48-1.73 1.73c-.47.13-1.33.22-2.65.28c-1.3.07-2.49.1-3.59.1L12 19c-4.19 0-6.8-.16-7.83-.44c-.9-.25-1.48-.83-1.73-1.73c-.13-.47-.22-1.1-.28-1.9c-.07-.8-.1-1.49-.1-2.09L2 12c0-2.19.16-3.8.44-4.83c.25-.9.83-1.48 1.73-1.73c.47-.13 1.33-.22 2.65-.28c1.3-.07 2.49-.1 3.59-.1L12 5c4.19 0 6.8.16 7.83.44c.9.25 1.48.83 1.73 1.73" />
      </symbol>
      <symbol id="instagram" viewBox="0 0 24 24">
        <path fill="currentColor"
              d="M7.8 2h8.4C19.4 2 22 4.6 22 7.8v8.4a5.8 5.8 0 0 1-5.8 5.8H7.8C4.6 22 2 19.4 2 16.2V7.8A5.8 5.8 0 0 1 7.8 2m-.2 2A3.6 3.6 0 0 0 4 7.6v8.8C4 18.39 5.61 20 7.6 20h8.8a3.6 3.6 0 0 0 3.6-3.6V7.6C20 5.61 18.39 4 16.4 4zm9.65 1.5a1.25 1.25 0 0 1 1.25 1.25A1.25 1.25 0 0 1 17.25 8A1.25 1.25 0 0 1 16 6.75a1.25 1.25 0 0 1 1.25-1.25M12 7a5 5 0 0 1 5 5a5 5 0 0 1-5 5a5 5 0 0 1-5-5a5 5 0 0 1 5-5m0 2a3 3 0 0 0-3 3a3 3 0 0 0 3 3a3 3 0 0 0 3-3a3 3 0 0 0-3-3" />
      </symbol>
      <symbol id="twitter" viewBox="0 0 24 24">
        <path fill="currentColor"
              d="M22.46 6c-.77.35-1.6.58-2.46.69c.88-.53 1.56-1.37 1.88-2.38c-.83.5-1.75.85-2.72 1.05C18.37 4.5 17.26 4 16 4c-2.35 0-4.27 1.92-4.27 4.29c0 .34.04.67.11.98C8.28 9.09 5.11 7.38 3 4.79c-.37.63-.58 1.37-.58 2.15c0 1.49.75 2.81 1.91 3.56c-.71 0-1.37-.2-1.95-.5v.03c0 2.08 1.48 3.82 3.44 4.21a4.22 4.22 0 0 1-1.93.07a4.28 4.28 0 0 0 4 2.98a8.521 8.521 0 0 1-5.33 1.84c-.34 0-.68-.02-1.02-.06C3.44 20.29 5.7 21 8.12 21C16 21 20.33 14.46 20.33 8.79c0-.19 0-.37-.01-.56c.84-.6 1.56-1.36 2.14-2.23" />
      </symbol>
      <symbol id="linkedin" viewBox="0 0 24 24">
        <path fill="currentColor"
              d="M19 3a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2zm-.5 15.5v-5.3a3.26 3.26 0 0 0-3.26-3.26c-.85 0-1.84.52-2.32 1.3v-1.11h-2.79v8.37h2.79v-4.93c0-.77.62-1.4 1.39-1.4a1.4 1.4 0 0 1 1.4 1.4v4.93zM6.88 8.56a1.68 1.68 0 0 0 1.68-1.68c0-.93-.75-1.69-1.68-1.69a1.69 1.69 0 0 0-1.69 1.69c0 .93.76 1.68 1.69 1.68m1.39 9.94v-8.37H5.5v8.37z" />
      </symbol>
      <symbol id="fitness" viewBox="0 0 24 24">
        <path fill="currentColor"
              d="M20.57 14.86L22 13.43L20.57 12L17 15.57L8.43 7L12 3.43L10.57 2L9.14 3.43L7.71 2L5.57 4.14L4.14 2.71L2.71 4.14l1.43 1.43L2 7.71l1.43 1.43L2 10.57L3.43 12L7 8.43L15.57 17L12 20.57L13.43 22l1.43-1.43L16.29 22l2.14-2.14l1.43 1.43l1.43-1.43l-1.43-1.43L22 16.29z" />
      </symbol>
      <symbol id="laptop" viewBox="0 0 24 24">
        <path fill="currentColor"
              d="M4 6h16v10H4m16 2a2 2 0 0 0 2-2V6a2 2 0 0 0-2-2H4c-1.11 0-2 .89-2 2v10a2 2 0 0 0 2 2H0v2h24v-2z" />
      </symbol>
      <symbol id="camera" viewBox="0 0 24 24">
        <path fill="currentColor"
              d="M4 4h3l2-2h6l2 2h3a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2m8 3a5 5 0 0 0-5 5a5 5 0 0 0 5 5a5 5 0 0 0 5-5a5 5 0 0 0-5-5m0 2a3 3 0 0 1 3 3a3 3 0 0 1-3 3a3 3 0 0 1-3-3a3 3 0 0 1 3-3" />
      </symbol>
      <symbol id="handshake" viewBox="0 0 24 24">
        <path fill="currentColor"
              d="M21.71 8.71c1.25-1.25.68-2.71 0-3.42l-3-3c-1.26-1.25-2.71-.68-3.42 0L13.59 4H11C9.1 4 8 5 7.44 6.15L3 10.59v4l-.71.7c-1.25 1.26-.68 2.71 0 3.42l3 3c.54.54 1.12.74 1.67.74c.71 0 1.36-.35 1.75-.74l2.7-2.71H15c1.7 0 2.56-1.06 2.87-2.1c1.13-.3 1.75-1.16 2-2C21.42 14.5 22 13.03 22 12V9h-.59zM20 12c0 .45-.19 1-1 1h-1v1c0 .45-.19 1-1 1h-1v1c0 .45-.19 1-1 1h-4.41l-3.28 3.28c-.31.29-.49.12-.6.01l-2.99-2.98c-.29-.31-.12-.49-.01-.6L5 15.41v-4l2-2V11c0 1.21.8 3 3 3s3-1.79 3-3h7zm.29-4.71L18.59 9H11v2c0 .45-.19 1-1 1s-1-.55-1-1V8c0-.46.17-2 2-2h3.41l2.28-2.28c.31-.29.49-.12.6-.01l2.99 2.98c.29.31.12.49.01.6" />
      </symbol>
      <symbol id="speakerphone" viewBox="0 0 24 24">
        <path fill="currentColor"
              d="M14 3.23v2.06c2.89.86 5 3.54 5 6.71s-2.11 5.84-5 6.7v2.07c4-.91 7-4.49 7-8.77c0-4.28-3-7.86-7-8.77M16.5 12c0-1.77-1-3.29-2.5-4.03V16c1.5-.71 2.5-2.24 2.5-4M3 9v6h4l5 5V4L7 9z" />
      </symbol>
      <symbol id="pencil-box" viewBox="0 0 24 24">
        <path fill="currentColor"
              d="M19 19V5H5v14zm0-16a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2zm-2.3 6.35l-1 1l-2.05-2.05l1-1c.21-.22.56-.22.77 0l1.28 1.28c.22.21.22.56 0 .77M7 14.94l6.06-6.06l2.06 2.06L9.06 17H7z" />
      </symbol>
      <symbol id="tick-circle" viewBox="0 0 15 15">
        <path fill="currentColor" fill-rule="evenodd"
              d="M0 7.5a7.5 7.5 0 1 1 15 0a7.5 7.5 0 0 1-15 0m7.072 3.21l4.318-5.398l-.78-.624l-3.682 4.601L4.32 7.116l-.64.768z"
              clip-rule="evenodd" />
      </symbol>
      <symbol id="clock" viewBox="0 0 24 24">
        <path fill="currentColor"
              d="M15.098 12.634L13 11.423V7a1 1 0 0 0-2 0v5a1 1 0 0 0 .5.866l2.598 1.5a1 1 0 1 0 1-1.732M12 2a10 10 0 1 0 10 10A10.011 10.011 0 0 0 12 2m0 18a8 8 0 1 1 8-8a8.01 8.01 0 0 1-8 8" />
      </symbol>
      <symbol id="arrow-circle-right" viewBox="0 0 24 24">
        <path fill="currentColor"
              d="M15.71 12.71a1 1 0 0 0 .21-.33a1 1 0 0 0 0-.76a1 1 0 0 0-.21-.33l-3-3a1 1 0 0 0-1.42 1.42l1.3 1.29H9a1 1 0 0 0 0 2h3.59l-1.3 1.29a1 1 0 0 0 0 1.42a1 1 0 0 0 1.42 0ZM22 12a10 10 0 1 0-10 10a10 10 0 0 0 10-10M4 12a8 8 0 1 1 8 8a8 8 0 0 1-8-8" />
      </symbol>
      <symbol id="search" viewBox="0 0 24 24">
        <path fill="currentColor"
              d="M21.71 20.29L18 16.61A9 9 0 1 0 16.61 18l3.68 3.68a1 1 0 0 0 1.42 0a1 1 0 0 0 0-1.39M11 18a7 7 0 1 1 7-7a7 7 0 0 1-7 7" />
      </symbol>
      <symbol id="user-circle" viewBox="0 0 24 24">
        <path fill="currentColor"
              d="M12 2a10 10 0 0 0-7.35 16.76a10 10 0 0 0 14.7 0A10 10 0 0 0 12 2m0 18a8 8 0 0 1-5.55-2.25a6 6 0 0 1 11.1 0A8 8 0 0 1 12 20m-2-10a2 2 0 1 1 2 2a2 2 0 0 1-2-2m8.91 6A8 8 0 0 0 15 12.62a4 4 0 1 0-6 0A8 8 0 0 0 5.09 16A7.92 7.92 0 0 1 4 12a8 8 0 0 1 16 0a7.92 7.92 0 0 1-1.09 4" />
      </symbol>
      <symbol id="shopping-bag" viewBox="0 0 24 24">
        <path fill="currentColor"
              d="M19 7h-3V6a4 4 0 0 0-8 0v1H5a1 1 0 0 0-1 1v11a3 3 0 0 0 3 3h10a3 3 0 0 0 3-3V8a1 1 0 0 0-1-1m-9-1a2 2 0 0 1 4 0v1h-4Zm8 13a1 1 0 0 1-1 1H7a1 1 0 0 1-1-1V9h2v1a1 1 0 0 0 2 0V9h4v1a1 1 0 0 0 2 0V9h2Z" />
      </symbol>
      <symbol id="heart" viewBox="0 0 24 24">
        <path fill="currentColor"
              d="M20.16 5A6.29 6.29 0 0 0 12 4.36a6.27 6.27 0 0 0-8.16 9.48l6.21 6.22a2.78 2.78 0 0 0 3.9 0l6.21-6.22a6.27 6.27 0 0 0 0-8.84m-1.41 7.46l-6.21 6.21a.76.76 0 0 1-1.08 0l-6.21-6.24a4.29 4.29 0 0 1 0-6a4.27 4.27 0 0 1 6 0a1 1 0 0 0 1.42 0a4.27 4.27 0 0 1 6 0a4.29 4.29 0 0 1 .08 6Z" />
      </symbol>
    </defs>
  </svg>

  <div class="preloader-wrapper">
    <div class="preloader">
    </div>
  </div>

  <div class="offcanvas offcanvas-end" data-bs-scroll="true" tabindex="-1" id="offcanvasCart" aria-labelledby="My Cart">
    <div class="offcanvas-header mt-3">
      <button type="button" class="btn-close shadow-none" data-bs-dismiss="offcanvas" aria-label="Close"></button>
    </div>
    <div class="offcanvas-body">
      <div class="order-md-last">
        <h4 class="d-flex justify-content-between align-items-center mb-4">
          <span>Giỏ hàng</span>
<!--          <span class="badge bg-primary rounded-circle pt-2 text-white">0</span>-->
        </h4>

        <ul class="list-group mb-4">
<!--          <li class="list-group-item d-flex justify-content-between align-items-center py-3 lh-sm">-->
<!--            <div>-->
<!--              <h6 class="my-0">Marketing Course</h6>-->
<!--              <small class="text-body-secondary">Brief description</small>-->
<!--            </div>-->
<!--            <span class="text-body-secondary">$120</span>-->
<!--          </li>-->
<!--          <li class="list-group-item d-flex justify-content-between align-items-center py-3 lh-sm">-->
<!--            <div>-->
<!--              <h6 class="my-0">Strategy Course</h6>-->
<!--              <small class="text-body-secondary">Brief description</small>-->
<!--            </div>-->
<!--            <span class="text-body-secondary">$80</span>-->
<!--          </li>-->
<!--          <li class="list-group-item d-flex justify-content-between align-items-center py-3 lh-sm">-->
<!--            <div>-->
<!--              <h6 class="my-0">Digital Course</h6>-->
<!--              <small class="text-body-secondary">Brief description</small>-->
<!--            </div>-->
<!--            <span class="text-body-secondary">$50</span>-->
<!--          </li>-->
<!--          <li class="list-group-item d-flex justify-content-between align-items-center py-3">-->
<!--            <span class="fw-bold">Total (USD)</span>-->
<!--            <strong>$250</strong>-->
<!--          </li>-->
        </ul>

<!--        <div class="d-grid my-5">-->
<!--          <button class="btn btn-primary px-5 py-3" type="submit">Mua ngay</button>-->
<!--        </div>-->
      </div>
    </div>
  </div>

  <div class="offcanvas offcanvas-end" data-bs-scroll="true" tabindex="-1" id="offcanvasSearch"
       aria-labelledby="Search">
    <div class="offcanvas-header mt-3">
      <button type="button" class="btn-close shadow-none" data-bs-dismiss="offcanvas" aria-label="Close"></button>
    </div>
    <div class="offcanvas-body">

      <div class="order-md-last">
        <h4 class="text-primary text-uppercase mb-3">Tìm kiếm</h4>
        <div class="search-bar border rounded-2 border-dark-subtle">
          <form id="search-form" class="text-center d-flex align-items-center" action="" method="">
            <input type="text" class="form-control border-0 bg-transparent" placeholder="Nhập từ khóa cần tìm" />
            <iconify-icon icon="tabler:search" class="fs-4 me-3"></iconify-icon>
          </form>
        </div>
      </div>
    </div>
  </div>

  <section id="top-nav" class="bg-secondary">
    <div class="text-center px-md-3 py-md-2">
      <div class="d-flex justify-content-between text-white py-1 m-0">
        <nav class="header-nav">
          <ul class="d-flex align-items-center">

            <li class="nav-item dropdown">
              <router-link to="/store/create" class="text-white " style="text-decoration: none;">Trở thành Nhà Bán Thuốc</router-link>
            </li>
            <li class="m-1 "> | </li>
            <li class="nav-item dropdown">
              <div class="btn-group">
                <a class="nav-link d-flex align-items-center pe-0 text-white" href="#" data-bs-toggle="dropdown">
                  <span class="dropdown-toggle">Kênh nhà thuốc</span>
                </a>
                <ul v-if="auth" class="dropdown-menu dropdown-menu-end dropdown-menu-arrow profile" >
                  <li class="dropdown-header">Chọn tài khoản cần chuyển đổi</li>
                  <li>
                    <hr class="dropdown-divider">
                  </li>
                  <li v-if="listMyStore" v-for="(item, index) in listMyStore" :key="index">
                    <a class="dropdown-item d-flex align-items-center" @click="openModal(item)">
                      <span>{{item.name}}</span>
                    </a>
                  </li>
                </ul>
              </div>
            </li>
          </ul>
        </nav>
        <nav class="header-nav ms-auto">
          <ul class="d-flex align-items-center">
            <li class="nav-item dropdown">
              <a class="nav-link nav-icon text-white show" href="#" data-bs-toggle="dropdown" aria-expanded="true">
                <i class="bi bi-bell"></i>
                <span class="badge bg-primary badge-number">0</span>
              </a>

<!--              <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow notifications" style="position: absolute; inset: 0px 0px auto auto; margin: 0px; transform: translate3d(-24.8px, 35.2px, 0px);" data-popper-placement="bottom-end">-->
<!--                <li class="dropdown-header">-->
<!--                  You have 4 new notifications-->
<!--                  <a href="#"><span class="badge rounded-pill bg-primary p-2 ms-2">View all</span></a>-->
<!--                </li>-->
<!--                <li>-->
<!--                  <hr class="dropdown-divider">-->
<!--                </li>-->

<!--                <li class="notification-item">-->
<!--                  <i class="bi bi-exclamation-circle text-warning"></i>-->
<!--                  <div>-->
<!--                    <h4>Lorem Ipsum</h4>-->
<!--                    <p>Quae dolorem earum veritatis oditseno</p>-->
<!--                    <p>30 min. ago</p>-->
<!--                  </div>-->
<!--                </li>-->

<!--                <li>-->
<!--                  <hr class="dropdown-divider">-->
<!--                </li>-->

<!--                <li class="notification-item">-->
<!--                  <i class="bi bi-x-circle text-danger"></i>-->
<!--                  <div>-->
<!--                    <h4>Atque rerum nesciunt</h4>-->
<!--                    <p>Quae dolorem earum veritatis oditseno</p>-->
<!--                    <p>1 hr. ago</p>-->
<!--                  </div>-->
<!--                </li>-->

<!--                <li>-->
<!--                  <hr class="dropdown-divider">-->
<!--                </li>-->

<!--                <li class="notification-item">-->
<!--                  <i class="bi bi-check-circle text-success"></i>-->
<!--                  <div>-->
<!--                    <h4>Sit rerum fuga</h4>-->
<!--                    <p>Quae dolorem earum veritatis oditseno</p>-->
<!--                    <p>2 hrs. ago</p>-->
<!--                  </div>-->
<!--                </li>-->

<!--                <li>-->
<!--                  <hr class="dropdown-divider">-->
<!--                </li>-->

<!--                <li class="notification-item">-->
<!--                  <i class="bi bi-info-circle text-primary"></i>-->
<!--                  <div>-->
<!--                    <h4>Dicta reprehenderit</h4>-->
<!--                    <p>Quae dolorem earum veritatis oditseno</p>-->
<!--                    <p>4 hrs. ago</p>-->
<!--                  </div>-->
<!--                </li>-->

<!--                <li>-->
<!--                  <hr class="dropdown-divider">-->
<!--                </li>-->
<!--                <li class="dropdown-footer">-->
<!--                  <a href="#">Show all notifications</a>-->
<!--                </li>-->

<!--              </ul>&lt;!&ndash; End Notification Dropdown Items &ndash;&gt;-->

            </li><!-- End Notification Nav -->

<!--            <li class="nav-item dropdown">-->

<!--              <a class="nav-link nav-icon text-white" href="#" data-bs-toggle="dropdown">-->
<!--                <i class="bi bi-chat-left-text"></i>-->
<!--                <span class="badge bg-success badge-number">3</span>-->
<!--              </a>&lt;!&ndash; End Messages Icon &ndash;&gt;-->

<!--              <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow messages">-->
<!--                <li class="dropdown-header">-->
<!--                  You have 3 new messages-->
<!--                  <a href="#"><span class="badge rounded-pill bg-primary p-2 ms-2">View all</span></a>-->
<!--                </li>-->
<!--                <li>-->
<!--                  <hr class="dropdown-divider">-->
<!--                </li>-->

<!--                <li class="message-item">-->
<!--                  <a href="#">-->
<!--                    <img src="" alt="" class="rounded-circle">-->
<!--                    <div>-->
<!--                      <h4>Maria Hudson</h4>-->
<!--                      <p>Velit asperiores et ducimus soluta repudiandae labore officia est ut...</p>-->
<!--                      <p>4 hrs. ago</p>-->
<!--                    </div>-->
<!--                  </a>-->
<!--                </li>-->
<!--                <li>-->
<!--                  <hr class="dropdown-divider">-->
<!--                </li>-->

<!--                <li class="message-item">-->
<!--                  <a href="#">-->
<!--                    <img src="" alt="" class="rounded-circle">-->
<!--                    <div>-->
<!--                      <h4>Anna Nelson</h4>-->
<!--                      <p>Velit asperiores et ducimus soluta repudiandae labore officia est ut...</p>-->
<!--                      <p>6 hrs. ago</p>-->
<!--                    </div>-->
<!--                  </a>-->
<!--                </li>-->
<!--                <li>-->
<!--                  <hr class="dropdown-divider">-->
<!--                </li>-->

<!--                <li class="message-item">-->
<!--                  <a href="#">-->
<!--                    <img src="" alt="" class="rounded-circle">-->
<!--                    <div>-->
<!--                      <h4>David Muldon</h4>-->
<!--                      <p>Velit asperiores et ducimus soluta repudiandae labore officia est ut...</p>-->
<!--                      <p>8 hrs. ago</p>-->
<!--                    </div>-->
<!--                  </a>-->
<!--                </li>-->
<!--                <li>-->
<!--                  <hr class="dropdown-divider">-->
<!--                </li>-->

<!--                <li class="dropdown-footer">-->
<!--                  <a href="#">Show all messages</a>-->
<!--                </li>-->

<!--              </ul>&lt;!&ndash; End Messages Dropdown Items &ndash;&gt;-->

<!--            </li>&lt;!&ndash; End Messages Nav &ndash;&gt;-->

            <li class="nav-item dropdown pe-3" v-if="auth">

              <a class="nav-link nav-profile d-flex align-items-center pe-0 text-white" href="#" data-bs-toggle="dropdown">
                <svg xmlns="http://www.w3.org/2000/svg" width="22px" height="22px">
                  <use href="#user-circle" />
                </svg>
                <span class="d-none d-md-block dropdown-toggle ps-2">{{message}}</span>
              </a>

              <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow profile">
                <li class="dropdown-header">
                  <h6>{{message}}</h6>
                  <span>{{role}}</span>
                </li>
                <li>
                  <hr class="dropdown-divider">
                </li>
                <li>
                  <a class="dropdown-item d-flex align-items-center" href="">
                    <i class="bi bi-person"></i>
                    <span>Thông tin cá nhân</span>
                  </a>
                </li>
                <li>
                  <hr class="dropdown-divider">
                </li>
                <li>
                  <router-link to="/" @click="logout" class="dropdown-item d-flex align-items-center" style="text-decoration: none">
                    <i class="bi bi-box-arrow-right"></i>Đăng xuất
                  </router-link>
                </li>
              </ul>
            </li>
            <li class="nav-item" v-if="!auth">
              <router-link to="/register"  class="text-white " style="text-decoration: none;">Đăng Ký</router-link>
              <span> | </span>
              <router-link to="/login" class="text-white" style="text-decoration: none;">Đăng Nhập</router-link>
            </li>
          </ul>
        </nav>
      </div>
    </div>
  </section>

  <nav class="main-menu d-flex navbar navbar-expand-lg p-2 py-3 p-lg-4 py-lg-4">
    <div class="container-fluid">
      <div class="main-logo d-lg-none">
        <router-link to="/"  class="text-white">
          thegioithuoc.com
        </router-link>
      </div>
      <button class="navbar-toggler shadow-none" type="button" data-bs-toggle="offcanvas"
              data-bs-target="#offcanvasNavbar" aria-controls="offcanvasNavbar">
        <span class="navbar-toggler-icon"></span>
      </button>

      <div class="offcanvas offcanvas-end" tabindex="-1" id="offcanvasNavbar" aria-labelledby="offcanvasNavbarLabel">

        <div class="offcanvas-header mt-3">
          <button type="button" class="btn-close shadow-none" data-bs-dismiss="offcanvas" aria-label="Close"></button>
        </div>

        <div class="offcanvas-body justify-content-between">
          <div class="main-logo">
            <router-link to="/" class="text-white fs-3" style="text-decoration: none;">
              thegioithuoc.com
            </router-link>
          </div>

          <ul class="navbar-nav menu-list list-unstyled align-items-lg-center d-flex gap-md-3 mb-0">
            <li class="nav-item">
              <router-link to="/" class="nav-link mx-2 active text-white">Trang Chủ</router-link>
            </li>
            <li class="nav-item">
              <router-link to="/" class="nav-link mx-2 active text-white">Sản phẩm</router-link>
            </li>
            <li class="nav-item" v-if="role === 'STORE'">
              <router-link to="/" class="nav-link mx-2 active text-white">Thống kê</router-link>
            </li>
          </ul>
          <div class="d-none d-lg-flex align-items-center">
            <ul class="d-flex  align-items-center list-unstyled m-0">
              <li>
                <a href="" class="ms-3 text-white">
                  <svg xmlns="http://www.w3.org/2000/svg" width="22px" height="22px">
                    <use href="#user-circle" />
                  </svg> </a>
              </li>
<!--              <li>-->
<!--                <a href="" class="ms-3 text-white">-->
<!--                  <svg xmlns="http://www.w3.org/2000/svg" width="22px" height="22px">-->
<!--                    <use href="#heart" />-->
<!--                  </svg> </a>-->
<!--              </li>-->

              <li v-if="role === 'CUSTOMER'">
                <a href="#" class="ms-3 text-white" data-bs-toggle="offcanvas" data-bs-target="#offcanvasCart"
                   aria-controls="offcanvasCart">
                  <svg xmlns="http://www.w3.org/2000/svg" width="22px" height="22px">
                    <use href="#shopping-bag" />
                  </svg> </a>
              </li>

              <li>
                <a href="#" class="ms-3 text-white" data-bs-toggle="offcanvas" data-bs-target="#offcanvasSearch"
                   aria-controls="offcanvasSearch">
                  <svg xmlns="http://www.w3.org/2000/svg" width="22px" height="22px">
                    <use href="#search" />
                  </svg> </a>

              </li>

            </ul>
          </div>

        </div>
      </div>

    </div>
    <div class="container-fluid d-lg-none">
      <div class="d-flex  align-items-end mt-3">
        <ul class="d-flex  align-items-center list-unstyled m-0">
          <li>
            <a href="account.html" class="me-3">
              <svg xmlns="http://www.w3.org/2000/svg" width="22px" height="22px">
                <use href="#user-circle" />
              </svg> </a>
          </li>
          <li>
            <a href="wishlist.html" class="me-3">
              <svg xmlns="http://www.w3.org/2000/svg" width="22px" height="22px">
                <use href="#heart" />
              </svg> </a>
          </li>

          <li class="">
            <a href="#" class="me-3" data-bs-toggle="offcanvas" data-bs-target="#offcanvasCart"
               aria-controls="offcanvasCart">
              <svg xmlns="http://www.w3.org/2000/svg" width="22px" height="22px">
                <use href="#shopping-bag" />
              </svg> </a>
          </li>

          <li>
            <a href="#" class="me-3" data-bs-toggle="offcanvas" data-bs-target="#offcanvasSearch"
               aria-controls="offcanvasSearch">
              <svg xmlns="http://www.w3.org/2000/svg" width="22px" height="22px">
                <use href="#search" />
              </svg> </a>
          </li>

        </ul>
      </div>
    </div>
  </nav>
  <div class="modal" tabindex="-1" role="dialog" id="passwordModal">
    <div class="modal-dialog" role="document">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Nhập mật khẩu</h5>
        </div>
        <div class="modal-body">
          <input type="password" v-model="password" class="form-control" placeholder="Mật khẩu">
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-primary" @click="changeAccount(selectedItem, password)">Xác nhận</button>
          <button type="button" class="btn btn-secondary" @click="closeTab" data-dismiss="modal">Đóng</button>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import {computed, onMounted, ref} from "vue";
import axios from "axios";
import {useStore} from "vuex";
import { useToast } from 'vue-toastification'
import AuthService from "@/services/auth.service.js";
import {useRouter} from "vue-router";
import StoreService from "@/services/store.service.js";
import ImageService from "@/services/image.service.js";
export default {
  name: "NavbarComponent",
  setup(){
    const toast = useToast()
    const store = useStore();
    const router = useRouter();
    const role = computed(() => store.state.role);
    const auth =computed(()=>store.state.auth);
    const message = computed(() => store.state.message);
    let listItem = ref([])
    let listMyStore = ref([])
    onMounted(async ()=>{
      await StoreService.getMyStore().then(response=>{
        listItem.value = response.data
        for(let item of listItem.value){
          ImageService.getOne(item.avatar).then(res=>{
            item.avatar = res.data
            listMyStore.value.push(item)
          })
        }
      })
    })
    const logout = async () =>{
      await AuthService.logout().then(async response => {
        localStorage.removeItem('token');
        axios.defaults.headers.common['Authorization'] = '';
        await store.dispatch('setMessage','')
        await store.dispatch('setAuth',false);
        toast.success("Đăng xuất thành công.")
        await router.push('/');
      }).catch(error => {
        console.log(error.response.data)
      })
    }
    return {
      listItem,listMyStore,
      auth,
      toast,
      role,
      logout,
      message
    }
  },
  data() {
    return {
      password: '',
      item:'',
      selectedItem: null
    };
  },
  methods:{
    openModal(item) {
      this.selectedItem = item;
      $('#passwordModal').modal('show');
    },
    closeTab(){
      $('#passwordModal').modal('hide');
    },
    async changeAccount(item, password){
      this.item = {
        password: password, type: 'STORE', account: item.id
      }
      console.log(item.id)
      $('#passwordModal').modal('hide');
      await AuthService.switch(this.item).then(res =>{
        this.toast.success(res.data.message)
      }).catch(err=>{
        // this.toast.error("Chuyển đổi tài khoản thất bại.")
        this.toast.error(err.response.data.detail)
      })
    }

  }
}
</script>
<style scoped>

</style>