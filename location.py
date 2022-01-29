# from selenium import webdriver
# from selenium.webdriver.chrome.service import Service
# from selenium.webdriver.common.by import By
# import time
# import os

# path = os.getcwd()

# option = webdriver.ChromeOptions()
# option.add_argument('headless')
# service = Service(executable_path='{}/chromedriver'.format(path))
# browser = webdriver.Chrome(service=service)

# def get_location():

#     #Truy cập trang web my-location.org để lấy tạo độ hiện tại
#     browser.get('https://my-location.org/')

#     #Chờ 10s để tìm vị trí
#     time.sleep(10)

#     #Lấy tạo độ kinh tuyến và vĩ tuyến
#     latitude = browser.find_element(By.ID, 'latitude').text
#     longitude = browser.find_element(By.ID, 'longitude').text

#     return latitude, longitude

# print(get_location())

import requests
req = requests.get('https://my-location.org/')

print(req.json())
